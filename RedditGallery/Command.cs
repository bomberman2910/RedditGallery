using System;
using System.Windows.Input;

namespace RedditGallery;

public class Command : ICommand
{
    public Command(Action commandAction)
    {
        CommandAction = commandAction;
    }

    public Action CommandAction { get; }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        CommandAction();
    }
}

public class Command<T> : ICommand
{
    public Command(Action<T> commandAction)
    {
        CommandAction = commandAction;
    }

    public Action<T> CommandAction { get; }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        CommandAction((T)parameter);
    }
}