using System.Windows.Input;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface IMainPageViewModel : IViewModel
{
    ICommand StartCommand {get;}
}

internal sealed class MainPageViewModel : ViewModelBase, IMainPageViewModel
{
    private ICommand _startCommand;
    public ICommand StartCommand => _startCommand ?? new Command(Start);

    private static void Start()
    {
        Navigate<ISettingsPage>();
    }
}