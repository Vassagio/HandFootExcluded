using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Events;

namespace HandFootExcluded;

public enum GameState
{
    None,
    Started,
    Summary,
    Finished
}

public interface IMainPageViewModel
{
    IGame Game { get; }
    IRound CurrentRound { get; }
    GameState GameState { get; }
    
    Command NextRoundCommand { get; }
    Command PreviousRoundCommand { get; }
    Command SummaryCommand { get; }
    Command NewCommand { get; }
    Command CloseCommand { get; }
}

internal sealed class MainPageViewModel : BindableItem, IMainPageViewModel
{
    private readonly IGameService _gameService;

    private IGame _game;
    private IRound _currentRound;
    private GameState _gameState;

    private Command _nextRoundCommand;
    private Command _previousRoundCommand;
    private Command _summaryCommand;
    private Command _newCommand;
    private Command _closeCommand;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IRound CurrentRound { get => _currentRound; set => SetProperty(ref _currentRound, value); }
    public GameState GameState { get => _gameState; set => SetProperty(ref _gameState, value); }

    public Command NextRoundCommand => _nextRoundCommand ?? new Command(NextRound);
    public Command PreviousRoundCommand => _previousRoundCommand ?? new Command(PreviousRound);
    public Command SummaryCommand => _summaryCommand ?? new Command(Summary);
    public Command NewCommand => _newCommand ?? new Command(New);
    public Command CloseCommand => _closeCommand ?? new Command(Close);

    public MainPageViewModel(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        GameState = GameState.None;

        EventAggregator.Instance.RegisterHandler<Players>(OnPlayersCreated);
        EventAggregator.Instance.RegisterHandler<NewGameEvent>(OnNewGame);
        EventAggregator.Instance.RegisterHandler<CloseSummaryEvent>(OnCloseSummary);
    }

    private void OnCloseSummary(CloseSummaryEvent obj)
    {
        GameState = GameState.Started;
    }

    private void OnPlayersCreated(Players players)
    {
        Game = _gameService.Create(players.ToList());
        CurrentRound = Game.First();
        GameState = GameState.Started;
        EventAggregator.Instance.SendMessage(PlayerScoreEvent.Yes);
    }

    private void OnNewGame(NewGameEvent newGameEvent)
    {
        CurrentRound = null;
        Game = null;
        GameState = GameState.None;
    }

    private void Summary()
    {
        GameState = GameState.Summary;
        EventAggregator.Instance.SendMessage(new GameFinishedEvent(Game));
    }

    private void NextRound()
    {
        var nextRound = _game.SingleOrDefault(r => r.Index == _currentRound.Index + 1);
        if (nextRound != null)
            CurrentRound = nextRound;
    }

    private void PreviousRound()
    {
        var previousRound = _game.SingleOrDefault(r => r.Index == _currentRound.Index - 1);
        if (previousRound != null)
            CurrentRound = previousRound;
    }

    private static void New() => EventAggregator.Instance.SendMessage(NewGameEvent.Instance);

    private static void Close() => Application.Current?.Quit();

    protected override void RefreshCommands()
    {
        base.RefreshCommands();

        NextRoundCommand.ChangeCanExecute();
        PreviousRoundCommand.ChangeCanExecute();
        SummaryCommand.ChangeCanExecute();
        NewCommand.ChangeCanExecute();
        CloseCommand.ChangeCanExecute();
    }
}