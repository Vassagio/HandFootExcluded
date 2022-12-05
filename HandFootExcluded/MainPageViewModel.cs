using Bertuzzi.MAUI.EventAggregator;

namespace HandFootExcluded;

public enum GameState
{
    None,
    Started,
    Finished
}

public interface IMainPageViewModel
{
    IGame Game { get; }
    IRound CurrentRound { get; }
    GameState GameState { get; }
    
    Command NextRoundCommand { get; }
    Command PreviousRoundCommand { get; }
    Command FinishCommand { get; }
}

internal sealed class MainPageViewModel : BindableItem, IMainPageViewModel
{
    private readonly IGameService _gameService;

    private IGame _game;
    private IRound _currentRound;
    private GameState _gameState;

    private Command _nextRoundCommand;
    private Command _previousRoundCommand;
    private Command _finishCommand;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IRound CurrentRound { get => _currentRound; set => SetProperty(ref _currentRound, value); }
    public GameState GameState { get => _gameState; set => SetProperty(ref _gameState, value); }

    public Command NextRoundCommand => _nextRoundCommand ?? new Command(NextRound);
    public Command PreviousRoundCommand => _previousRoundCommand ?? new Command(PreviousRound);
    public Command FinishCommand => _finishCommand ?? new Command(Finish);

    public MainPageViewModel(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        GameState = GameState.None;

        EventAggregator.Instance.RegisterHandler<Players>(OnPlayersCreated);
        EventAggregator.Instance.RegisterHandler<NewGameEvent>(OnNewGame);
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

    private void Finish()
    {
        GameState = GameState.Finished;
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

    protected override void RefreshCommands()
    {
        base.RefreshCommands();

        NextRoundCommand.ChangeCanExecute();
        PreviousRoundCommand.ChangeCanExecute();
        FinishCommand.ChangeCanExecute();
    }
}