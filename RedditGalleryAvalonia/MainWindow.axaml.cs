using System;
using Avalonia.Controls;

namespace RedditGalleryAvalonia
{
    public partial class MainWindow : Window
    {
        private bool showText;
        private bool fatal;
        private string messageText;

        public MainWindow()
        {
            InitializeComponent();
            Opened += async (sender, args) =>
            {
                if (!showText)
                    return;
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Error", messageText);
                _ = await messageBoxStandardWindow.Show();
                if(fatal)
                    Environment.Exit(1);
            };
            DataContext = new MainViewModel(this);
        }

        public void ShowMessageBox(string text, bool fatal)
        {
            this.fatal = fatal;
            showText = true;
            messageText = text;
        }

        
    }
}