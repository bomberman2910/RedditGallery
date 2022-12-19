using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;

namespace RedditGalleryAvalonia;

public class SettingsViewModel : ViewModelBase
{
    public ICommand AddSubRedditCommand { get; }
    public ICommand RemoveSubRedditCommand { get; }

    public ObservableCollection<string> SubRedditList { get; }
    public string SelectedSubReddit { get; set; }

    public SettingsViewModel(ObservableCollection<string> subRedditList)
    {
        AddSubRedditCommand = new Command<Window>(OnAddSubReddit);
        RemoveSubRedditCommand = new Command(OnRemoveSubReddit);

        SubRedditList = subRedditList;
    }

    private void OnRemoveSubReddit()
    {
        if(string.IsNullOrEmpty(SelectedSubReddit))
            return;
        SubRedditList.Remove(SelectedSubReddit);
        SelectedSubReddit = null;
        RaisePropertyChanged(nameof(SubRedditList));
    }

    private async void OnAddSubReddit(Window window)
    {
        var textInput = new TextInput();
        await textInput.ShowDialog(window);
        if(textInput.Result == DialogResult.Ok)
            SubRedditList.Add($"r/{textInput.Text}");
    }
}