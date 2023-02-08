using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HandFootExcluded.Common;

public interface IBindableItem { }

public abstract class BindableItem : INotifyPropertyChanged, IBindableItem
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    protected bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);

        return true;
    }

    protected bool SetProperty<T>(ref T storage, T value, Action<T> onChanged, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        onChanged?.Invoke(value);
        OnPropertyChanged(propertyName);

        return true;
    }

    protected ICommand SetCommand(ref ICommand command, Action execute)
    {
        command ??= new Command(execute);
        return command;
    }

    protected ICommand SetCommand(ref ICommand command, Action execute, Func<bool> canExecute)
    {
        command ??= new Command(execute, canExecute);
        return command;
    }

    protected ICommand SetCommand<T>(ref ICommand command, Action<T> execute)
    {
        command ??= new Command<T>(execute);
        return command;
    }

    protected ICommand SetCommand<T>(ref ICommand command, Action<T> execute, Func<T, bool> canExecute) 
    {
        command ??= new Command<T>(execute, canExecute);
        return command;
    }

    private void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        RefreshCommands();
    }

    protected virtual void RefreshCommands() { }
}

