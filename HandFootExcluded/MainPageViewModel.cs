using Bertuzzi.MAUI.EventAggregator;

namespace HandFootExcluded;

public interface IMainPageViewModel
{
    IGame Game { get; }
    IRound CurrentRound { get; }
    bool HasGameStarted { get; }

    Command NextRoundCommand { get; }
    Command PreviousRoundCommand { get; }
    Command FinishCommand { get; }
}

internal sealed class MainPageViewModel : BindableItem, IMainPageViewModel
{
    private readonly IGameService _gameService;

    private IGame _game;
    private IRound _currentRound;
    
    private bool _hasGameStarted;

    private Command _nextRoundCommand;
    private Command _previousRoundCommand;
    private Command _finishCommand;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IRound CurrentRound { get => _currentRound; set => SetProperty(ref _currentRound, value); }
    

    public bool HasGameStarted { get => _hasGameStarted; set => SetProperty(ref _hasGameStarted, value); }

    public Command NextRoundCommand => _nextRoundCommand ?? new Command(NextRound);
    public Command PreviousRoundCommand => _previousRoundCommand ?? new Command(PreviousRound);
    public Command FinishCommand => _finishCommand ?? new Command(Finish);

    public MainPageViewModel(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        EventAggregator.Instance.RegisterHandler<Players>(OnPlayersCreated);
    }

    private void OnPlayersCreated(Players players)
    {
        Game = _gameService.Create(players.ToList());
        CurrentRound = Game.First();
        HasGameStarted = true;
        EventAggregator.Instance.SendMessage("Score");
    }

    private void Finish() { }

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