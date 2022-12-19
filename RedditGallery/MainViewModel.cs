using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RedditGallery.Base;

namespace RedditGallery;

public class MainViewModel : INotifyPropertyChanged
{
    private Post currentPost;
    private BitmapSource imageSource;
    private string title;
    private bool zoomedIn;
    private string category;
    private readonly RedditApiController apiController;

    public MainViewModel()
    {
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

            CurrentPost = apiController.GetPostAsync().Result;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Environment.Exit(1);
        }

        while (!CurrentPost.Type.Equals("image"))
            CurrentPost = apiController.GetPostAsync(after: CurrentPost.Name).Result;
    }

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
            OnPropertyChanged();
            OnPropertyChanged(nameof(ZoomButtonText));
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
            OnPropertyChanged();
            apiController.CurrentPostCategory = Enum.Parse<PostCategory>(category);
        }
    }

    public string Title
    {
        get { return title; }
        set
        {
            title = value;
            OnPropertyChanged();
        }
    }
    
    public BitmapSource ImageSource
    {
        get { return imageSource; }
        set
        {
            imageSource = value;
            OnPropertyChanged();
        }
    }

    public Post CurrentPost
    {
        get { return currentPost; }
        set
        {
            currentPost = value;
            OnPropertyChanged();
            TryGetImageForPost();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

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
        var nextPost = apiController.GetPostAsync(after: CurrentPost.Name).Result;
        if (nextPost is null)
            return;
        while (!nextPost.Type.Equals("image") || nextPost.MediaLink.Contains(".gif"))
        {
            nextPost = apiController.GetPostAsync(after: nextPost.Name).Result;
            if (nextPost is null)
                return;
        }

        CurrentPost = nextPost;
    }

    private void OnPreviousImage()
    {
        var previousPost = apiController.GetPostAsync(before: CurrentPost.Name).Result;
        if (previousPost is null)
            return;
        while (!previousPost.Type.Equals("image") || previousPost.MediaLink.Contains(".gif"))
        {
            previousPost = apiController.GetPostAsync(before: previousPost.Name).Result;
            if (previousPost is null)
                return;
        }

        CurrentPost = previousPost;
    }

    private async void OnSelectCategory(string categoryParameter)
    {
        Category = categoryParameter;
        await apiController.GetPostAsync();
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

    private BitmapSource RetrieveImage(string url)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.UriSource = new Uri(url, UriKind.Absolute);
        bitmapImage.EndInit();
        return bitmapImage;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}