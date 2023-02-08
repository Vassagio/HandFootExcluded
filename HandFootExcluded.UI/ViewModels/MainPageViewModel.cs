using System.Windows.Input;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface IMainPageViewModel : IViewModel
{
    ICommand StartCommand { get; }
    ICommand GameHistoryCommand { get; }
}

internal sealed class MainPageViewModel : ViewModelBase, IMainPageViewModel
{
    private ICommand _gameHistoryCommand;
    private ICommand _startCommand;

    public ICommand StartCommand => SetCommand(ref _startCommand, Start);
    public ICommand GameHistoryCommand => SetCommand(ref _gameHistoryCommand, GameHistory);

    private static void Start() => Navigate<ISettingsPage>();

    private static void GameHistory() => Navigate<IGameHistoryPage>();
}