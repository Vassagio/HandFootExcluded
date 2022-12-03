using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandFootExcluded;

public abstract class BindableItem : INotifyPropertyChanged
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

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}