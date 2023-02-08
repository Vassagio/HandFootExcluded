using HandFootExcluded.UI.Services.GameHistoryServices;

namespace HandFootExcluded.UI.ViewModels;

public interface IGameHistoryPageViewModel : IViewModel
{
}

internal sealed class GameHistoryPageViewModel : ViewModelBase, IGameHistoryPageViewModel
{
    private readonly IGameHistoryService _gameHistoryService;

    public GameHistoryPageViewModel(IGameHistoryService gameHistoryService) => _gameHistoryService = gameHistoryService ?? throw new ArgumentNullException(nameof(gameHistoryService));
}