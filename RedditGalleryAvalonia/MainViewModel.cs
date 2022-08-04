using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            RaisePropertyChanged(nameof(IsImageLoading));
        }
    }

    public bool IsImageLoading
    {
        get { return ImageSource is null; }
    }

    public Post CurrentPost
    {
        get { return currentPost; }
        set
        {
            currentPost = value;
            Title = currentPost.Title;
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
            apiController = new RedditApiController();
            apiController.LoadApplicationState();
            Category = apiController.CurrentPostCategory.ToString();
            
            GetFirstPost();
        }
        catch (Exception ex)
        {
            mainWindow.ShowMessageBox(ex.Message, true);
        }
    }

    private async void GetFirstPost()
    {
        var firstPost = await apiController.GetPostAsync();
        while (!firstPost.Type.Equals("image"))
            firstPost = await apiController.GetPostAsync(after: firstPost.Name);
        CurrentPost = firstPost;
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
        ImageSource = null;
        TryGetImageForPost();
    }

    private async void OnNextImage()
    {
        Title = string.Empty;
        ImageSource = null;
        var nextPost = await apiController.GetPostAsync(after: CurrentPost.Name);
        if (nextPost is null)
            return;
        while (!nextPost.Type.Equals("image") || nextPost.MediaLink.Contains(".gif"))
        {
            nextPost = await apiController.GetPostAsync(after: nextPost.Name);
            if (nextPost is null)
                return;
        }

        CurrentPost = nextPost;
    }

    private async void OnPreviousImage()
    {
        Title = string.Empty;
        ImageSource = null;
        var previousPost = await apiController.GetPostAsync(CurrentPost.Name);
        if (previousPost is null)
            return;
        while (!previousPost.Type.Equals("image") || previousPost.MediaLink.Contains(".gif"))
        {
            previousPost = await apiController.GetPostAsync(previousPost.Name);
            if (previousPost is null)
                return;
        }

        CurrentPost = previousPost;
    }

    private void OnSelectCategory(string categoryParameter)
    {
        Category = categoryParameter;
        Title = string.Empty;
        ImageSource = null;
        GetFirstPost();
    }

    private async void TryGetImageForPost()
    {
        try
        {
            ImageSource = await RetrieveImageAsync(currentPost.MediaLink);
        }
        catch
        {
            ImageSource = null;
            Title = currentPost.Title + " (image loading error)";
        }
    }

    private async Task<Bitmap> RetrieveImageAsync(string url)
    {
        using var client = new HttpClient();
        var result = await client.GetStreamAsync(url);
        var bufferStream = new MemoryStream();
        await result.CopyToAsync(bufferStream);
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