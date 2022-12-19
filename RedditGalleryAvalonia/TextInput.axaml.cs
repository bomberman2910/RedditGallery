using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RedditGalleryAvalonia;

public partial class TextInput : Window
{
    private TextBox textBox;

    public DialogResult Result { get; set; } = DialogResult.Cancel;
    public string Text { get => textBox.Text; }

    public TextInput()
    {
        InitializeComponent();
        NameScope thisWindowNameScope = (NameScope)this.FindNameScope();
        textBox = (TextBox)thisWindowNameScope.Find("TextBox");
    }

    public void OnClickOkay(object sender, RoutedEventArgs e)
    {
        Result = DialogResult.Ok;
        Close();
    }
}

public enum DialogResult
{
    Ok,
    Cancel
}