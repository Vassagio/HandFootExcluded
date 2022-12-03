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
    bool HasGameStarted {get;}
    ICommand StartCommand { get; }
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
    private IGame _game;
    private bool _hasGameStarted;

    public string DefaultPlayer1 { get => _defaultPlayer1; set => SetProperty(ref _defaultPlayer1, value); }
    public string DefaultPlayer2 { get => _defaultPlayer2; set => SetProperty(ref _defaultPlayer2, value); }
    public string DefaultPlayer3 { get => _defaultPlayer3; set => SetProperty(ref _defaultPlayer3, value); }
    public string DefaultPlayer4 { get => _defaultPlayer4; set => SetProperty(ref _defaultPlayer4, value); }
    public string DefaultPlayer5 { get => _defaultPlayer5; set => SetProperty(ref _defaultPlayer5, value); }
    public IGame Game { get => _game; set => SetProperty(ref _game, value, OnGameChanged); }
    public bool HasGameStarted {get => _hasGameStarted; set => SetProperty(ref _hasGameStarted, value); }
    
    public ICommand StartCommand => new Command(Start, CanStart);

    public MainPageViewModel(IPlayerBuilder playerBuilder, IGameService gameService)
    {
        _playerBuilder = playerBuilder ?? throw new ArgumentNullException(nameof(playerBuilder));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
    }

    private void OnGameChanged()
    {
        HasGameStarted = _game is not null;
    }

    private bool CanStart() =>
        !string.IsNullOrWhiteSpace(_defaultPlayer1) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer2) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer3) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer4) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer5);

    private void Start()
    {
        var defaultPlayers = new List<string>
        {
            _defaultPlayer1,
            _defaultPlayer2,
            _defaultPlayer3,
            _defaultPlayer4,
            _defaultPlayer5,
        };

        defaultPlayers.Shuffle();

        var players = new Players();
        for (var i = 1; i <= defaultPlayers.Count; i++)
        {
            var player = _playerBuilder.WithPosition(i).WithName(defaultPlayers[i-1]).Build();
            players.Add(player);
        }

        Game = _gameService.Create(players.ToList());
    }
}