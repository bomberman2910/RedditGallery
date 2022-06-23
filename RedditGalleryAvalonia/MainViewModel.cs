using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using RedditGallery.Base;

namespace RedditGalleryAvalonia;

public class MainViewModel : ViewModelBase
{
    private readonly RedditApiController apiController;
    private string category;
    private Post currentPost;
    private Bitmap imageSource;
    private string title;
    private bool zoomedIn;
    private readonly MainWindow mainWindow;

    public ICommand PreviousImageCommand { get; }
    public ICommand NextImageCommand { get; }
    public ICommand ZoomCommand { get; }
    public ICommand GotoRedditCommand { get; }
    public ICommand SelectCategoryCommand { get; }

    public bool ZoomedIn
    {
        get { return zoomedIn; }
        set
        {
            zoomedIn = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ZoomButtonText));
        }
    }

    public string ZoomButtonText
    {
        get { return ZoomedIn ? "-" : "+"; }
    }

    public string Category
    {
        get { return category; }
        set
        {
            category = value;
            RaisePropertyChanged();
            apiController.CurrentPostCategory = Enum.Parse<PostCategory>(category);
        }
    }

    public string Title
    {
        get { return title; }
        set
        {
            title = value;
            RaisePropertyChanged();
        }
    }

    public Bitmap ImageSource
    {
        get { return imageSource; }
        set
        {
            imageSource = value;
            RaisePropertyChanged();
        }
    }

    public Post CurrentPost
    {
        get { return currentPost; }
        set
        {
            currentPost = value;
            RaisePropertyChanged();
            TryGetImageForPost();
        }
    }

    public MainViewModel(MainWindow window)
    {
        mainWindow = window;

        PreviousImageCommand = new Command(OnPreviousImage);
        NextImageCommand = new Command(OnNextImage);
        ZoomCommand = new Command(OnZoom);
        GotoRedditCommand = new Command(OnGotoReddit);
        SelectCategoryCommand = new Command<string>(OnSelectCategory);

        try
        {
            apiController = new RedditApiController
            {
                CurrentSubRedditLink = "r/unixporn"
            };

            Category = "New";

            CurrentPost = apiController.GetPost();
            while (!CurrentPost.Type.Equals("image"))
                CurrentPost = apiController.GetPost(after: CurrentPost.Name);
        }
        catch (Exception ex)
        {
            mainWindow.ShowMessageBox(ex.Message);
        }
    }

    private void OnGotoReddit()
    {
        var startInfo = new ProcessStartInfo($"https://www.reddit.com{currentPost.Link}")
        {
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    private void OnZoom()
    {
        ZoomedIn = !ZoomedIn;
    }

    private void OnNextImage()
    {
        var nextPost = apiController.GetPost(after: CurrentPost.Name);
        if (nextPost is null)
            return;
        while (!nextPost.Type.Equals("image") || nextPost.MediaLink.Contains(".gif"))
        {
            nextPost = apiController.GetPost(after: nextPost.Name);
            if (nextPost is null)
                return;
        }

        CurrentPost = nextPost;
    }

    private void OnPreviousImage()
    {
        var previousPost = apiController.GetPost(CurrentPost.Name);
        if (previousPost is null)
            return;
        while (!previousPost.Type.Equals("image") || previousPost.MediaLink.Contains(".gif"))
        {
            previousPost = apiController.GetPost(previousPost.Name);
            if (previousPost is null)
                return;
        }

        CurrentPost = previousPost;
    }

    private void OnSelectCategory(string categoryParameter)
    {
        Category = categoryParameter;
        apiController.GetPost();
    }

    private void TryGetImageForPost()
    {
        try
        {
            ImageSource = null;
            ImageSource = RetrieveImage(currentPost.MediaLink);
            Title = currentPost.Title;
        }
        catch
        {
            ImageSource = null;
            Title = currentPost.Title + " (image loading error)";
        }
    }

    private Bitmap RetrieveImage(string url)
    {
        using var client = new HttpClient();
        var result = client.GetStreamAsync(url).Result;
        var bufferStream = new MemoryStream();
        result.CopyTo(bufferStream);
        bufferStream.Position = 0;
        var bitmapImage = new Bitmap(bufferStream);
        result.Dispose();
        bufferStream.Dispose();
        return bitmapImage;
    }
}

public class ViewModelBase : INotifyPropertyChanged
{
    private readonly List<string> events = new();
    private PropertyChangedEventHandler propertyChanged;

    public event PropertyChangedEventHandler PropertyChanged
    {
        add
        {
            propertyChanged += value;
            events.Add("added");
        }
        remove
        {
            propertyChanged -= value;
            events.Add("removed");
        }
    }

    protected void OnPropertyChanged(PropertyChangedEventArgs e)
    {
    }

    protected bool RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        RaisePropertyChanged(propertyName);
        return true;
    }

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
        var e = new PropertyChangedEventArgs(propertyName);
        OnPropertyChanged(e);
        propertyChanged?.Invoke(this, e);
    }
}