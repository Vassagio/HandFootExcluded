using System.Runtime.CompilerServices;
using Bertuzzi.MAUI.EventAggregator;
using System.Windows.Input;

namespace HandFootExcluded;

public interface IMainPageViewModel
{
    string DefaultPlayer1 { get; }
    string DefaultPlayer2 { get; }
    string DefaultPlayer3 { get; }
    string DefaultPlayer4 { get; }
    string DefaultPlayer5 { get; }
    IGame Game {get;}
    IEnumerable<IPlayerScore> PlayerScores { get; }
    bool HasGameStarted {get;}
    Command StartCommand { get; }
    Command NextRoundCommand {get;}
    Command PreviousRoundCommand {get;}
    Command FinishCommand {get;}
}

internal sealed class MainPageViewModel : BindableItem, IMainPageViewModel
{
    private readonly IPlayerBuilder _playerBuilder;
    private readonly IGameService _gameService;
    private string _defaultPlayer1 = "William Christopher Chronowski";
    private string _defaultPlayer2 = "Tami Renae Chronowski";
    private string _defaultPlayer3 = "Kaelia Shyenne Chronowski";
    private string _defaultPlayer4 = "Korian Alexa Chronowski";
    private string _defaultPlayer5 = "Jay Michael Looney";
    private IList<string> _defaultPlayers = new List<string>();
    private IGame _game;
    private IRound _currentRound;
    private IEnumerable<IPlayerScore> _playerScores = Enumerable.Empty<IPlayerScore>();
    private bool _hasGameStarted;

    private Command _startCommand;
    private Command _nextRoundCommand;
    private Command _previousRoundCommand;
    private Command _finishCommand;

    public string DefaultPlayer1 { get => _defaultPlayer1; set => SetProperty(ref _defaultPlayer1, value); }
    public string DefaultPlayer2 { get => _defaultPlayer2; set => SetProperty(ref _defaultPlayer2, value); }
    public string DefaultPlayer3 { get => _defaultPlayer3; set => SetProperty(ref _defaultPlayer3, value); }
    public string DefaultPlayer4 { get => _defaultPlayer4; set => SetProperty(ref _defaultPlayer4, value); }
    public string DefaultPlayer5 { get => _defaultPlayer5; set => SetProperty(ref _defaultPlayer5, value); }
    public IGame Game { get => _game; set => SetProperty(ref _game, value, OnGameChanged); }
    public IRound CurrentRound {get => _currentRound; set => SetProperty(ref _currentRound, value);}
    public IEnumerable<IPlayerScore> PlayerScores { get => _playerScores; set => SetProperty(ref _playerScores, value); }

    public bool HasGameStarted {get => _hasGameStarted; set => SetProperty(ref _hasGameStarted, value); }
    
    public Command StartCommand => _startCommand ?? new Command(Start, CanStart);
    public Command NextRoundCommand => _nextRoundCommand ?? new Command(NextRound);
    public Command PreviousRoundCommand => _previousRoundCommand ?? new Command(PreviousRound);
    public Command FinishCommand => _finishCommand ?? new Command(Finish);

    public MainPageViewModel(IPlayerBuilder playerBuilder, IGameService gameService)
    {
        _playerBuilder = playerBuilder ?? throw new ArgumentNullException(nameof(playerBuilder));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        _defaultPlayers = new List<string>
        {
            _defaultPlayer1,
            _defaultPlayer2,
            _defaultPlayer3,
            _defaultPlayer4,
            _defaultPlayer5,
        };

        EventAggregator.Instance.RegisterHandler<IEnumerable<IPlayerScore>>(Score);
    }

    private void Score(IEnumerable<IPlayerScore> playerScores) => PlayerScores = playerScores.OrderByDescending(ps => ps.Score);

    private void OnGameChanged()
    {
        EventAggregator.Instance.RegisterHandler<IEnumerable<IPlayerScore>>(Score);
        HasGameStarted = _game is not null;
        EventAggregator.Instance.RegisterHandler<IEnumerable<IPlayerScore>>(Score);
    }

    private bool CanStart() =>
        !string.IsNullOrWhiteSpace(_defaultPlayer1) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer2) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer3) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer4) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer5);

    private void Start()
    {
        _defaultPlayers.Shuffle();

        var players = new Players();
        for (var i = 1; i <= _defaultPlayers.Count; i++)
        {
            var player = _playerBuilder.WithPosition(i).WithName(_defaultPlayers[i-1]).Build();
            players.Add(player);
        }

        Game = _gameService.Create(players.ToList());
        CurrentRound = Game.First();
        EventAggregator.Instance.SendMessage("Score");
    }

    private void Finish()
    {                
        Start();
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

        StartCommand.ChangeCanExecute();
        NextRoundCommand.ChangeCanExecute();
        PreviousRoundCommand.ChangeCanExecute();
        FinishCommand.ChangeCanExecute();
    }
}