using System.Text.Json.Serialization;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;
using System.Windows.Input;
using HandFootExcluded.Common;
using HandFootExcluded.Core;
using HandFootExcluded.UI.Services;

namespace HandFootExcluded.UI.ViewModels;

public interface IGamePageViewModel : IViewModel
{
    IGame Game { get; }
    IEnumerable<IRoundViewModel> Rounds { get; }
    IRoundViewModel CurrentRound {get;}
    ITotalScoreViewModel TotalScore { get; }
    IDictionary<int, int> IndividualBonusAmounts { get; }
    int TeamBonusAmount { get; }
    int MinDiscardPickup { get; }
    int MaxDiscardPickup { get; }

    ICommand SummaryCommand { get; }
    ICommand PreviousCommand { get; }
    ICommand NextCommand { get; }
    ICommand CloseCommand { get; }
    ICommand DiscardPickupCommand { get; }
}

internal sealed class GamePageViewModel : ViewModelBase, IGamePageViewModel
{
    private readonly IGameService _gameService;
    private readonly IScoringService _scoringService;
    private readonly IAlertService _alertService;
    private readonly IJsonService _jsonService;

    private IGame _game;
    private IEnumerable<IRoundViewModel> _rounds;
    private IRoundViewModel _currentRound;
    private ITotalScoreViewModel _totalScore;
    private IDictionary<int, int> _individualBonusAmounts;
    private int _teamBonusAmount;
    private int _minDiscardPickup;
    private int _maxDiscardPickup;

    private ICommand _summaryCommand;
    private ICommand _previousCommand;
    private ICommand _nextCommand;
    private ICommand _closeCommand;
    private ICommand _discardPickupCommand;

    [JsonIgnore]
    public IGame Game { get => _game; set => SetProperty(ref _game, value); }

    public IEnumerable<IRoundViewModel> Rounds { get => _rounds; set => SetProperty(ref _rounds, value); }
    public IRoundViewModel CurrentRound {get =>_currentRound; set => SetProperty(ref _currentRound, value);}
    public ITotalScoreViewModel TotalScore { get => _totalScore; set => SetProperty(ref _totalScore, value); }
    public IDictionary<int, int> IndividualBonusAmounts { get => _individualBonusAmounts; set => SetProperty(ref _individualBonusAmounts, value); }
    public int TeamBonusAmount { get => _teamBonusAmount; set => SetProperty(ref _teamBonusAmount, value); }
    public int MinDiscardPickup { get => _minDiscardPickup; set => SetProperty(ref _minDiscardPickup, value); }
    public int MaxDiscardPickup { get => _maxDiscardPickup; set => SetProperty(ref _maxDiscardPickup, value); }

    [JsonIgnore]
    public ICommand SummaryCommand => _summaryCommand ?? new Command(Summary);
    [JsonIgnore]
    public ICommand PreviousCommand => _previousCommand ?? new Command(Previous);
    [JsonIgnore]
    public ICommand NextCommand => _nextCommand ?? new Command(Next);
    [JsonIgnore]
    public ICommand CloseCommand => _closeCommand ?? new Command(Close);
    [JsonIgnore]
    public ICommand DiscardPickupCommand => _discardPickupCommand ?? new Command(DiscardPickup);

    public GamePageViewModel() { }

    public GamePageViewModel(IGameService gameService, IScoringService scoringService, IAlertService alertService, IJsonService jsonService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
        _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
        _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));

        _totalScore = new TotalScoreViewModel(new ScoreLines());

        EventAggregator.Instance.RegisterHandler<SettingsChangedEvent>(OnSettingsChanged);
        EventAggregator.Instance.RegisterHandler<GameLoadedEvent>(OnGameLoaded);
        EventAggregator.Instance.RegisterHandler<ScoreChangedEvent>(OnScoreChanged);
    }

    private async void OnScoreChanged(ScoreChangedEvent scoreChangedEvent)
    {
        var scoreLines = await _scoringService.Score(Game, Rounds, IndividualBonusAmounts, TeamBonusAmount);
        TotalScore = new TotalScoreViewModel(scoreLines.GetGrandTotalLines());
    }

    private void OnGameLoaded(GameLoadedEvent gameLoadedEvent)
    {
        if (gameLoadedEvent?.GamePageViewModel is not { } gamePageViewModel) return;
        if (gameLoadedEvent?.SettingsPageViewModel is not { } settingsPageViewModel) return;

        IndividualBonusAmounts = gamePageViewModel.IndividualBonusAmounts;
        TeamBonusAmount = gamePageViewModel.TeamBonusAmount;
        MinDiscardPickup = gamePageViewModel.MinDiscardPickup;
        MaxDiscardPickup = gamePageViewModel.MaxDiscardPickup;
        Game ??= _gameService.CreateGame(settingsPageViewModel.PlayerSettings, settingsPageViewModel.RoundOpenings);
        Rounds = gamePageViewModel.Rounds;
        CurrentRound = gamePageViewModel.CurrentRound;

        OnScoreChanged(new ScoreChangedEvent());
    }

    private void OnSettingsChanged(SettingsChangedEvent settingsChangedEvent)
    {
        IndividualBonusAmounts = settingsChangedEvent.IndividualBonusAmounts;
        TeamBonusAmount = settingsChangedEvent.TeamBonusAmount;
        MinDiscardPickup = settingsChangedEvent.MinDiscardPickup;
        MaxDiscardPickup = settingsChangedEvent.MaxDiscardPickup;
        Game ??= _gameService.CreateGame(settingsChangedEvent.Players, settingsChangedEvent.RoundOpeningAmounts);
        if (Rounds == null || !Rounds.Any())
        {
            var rounds = Game.Rounds.Select(ToViewModel).ToList();
            Rounds = rounds;
            CurrentRound = Rounds.FirstOrDefault();
        }

        OnScoreChanged(new ScoreChangedEvent());
    }

    private static IRoundViewModel ToViewModel(IRound round) =>
        new RoundViewModel()
        {
            Order = round.Order,
            OpenAmount = round.OpenAmount,
            StartingTeam = ToViewModel(round.Teams.Find<IStartingTeam>()),
            OpposingTeam = ToViewModel(round.Teams.Find<IOpposingTeam>()),
            ExcludedPlayerInitials = round.Players?.Find<IExcludedPlayer>()?.Initials ?? string.Empty
        };

    private static ITeamViewModel ToViewModel(ITeam team) =>
        new TeamViewModel()
        {
            TeamName = team.Name,
            PlayerInitials = team.Player.Initials,
            PartnerInitials = team.Partner.Initials
        };

    private void Summary()
    {
        var scoreLines = _scoringService.Score(Game, Rounds, IndividualBonusAmounts, TeamBonusAmount).Result;
        EventAggregator.Instance.PostMessage(new ScoreLinesChangedEvent(scoreLines));
        Navigate<ISummaryPage>();
    }

    private void Previous()
    {
        _jsonService.SaveAsync(this, "game.json");
        var previousRound = Rounds.SingleOrDefault(r => r.Order == _currentRound.Order - 1);
        if (previousRound != null)
            CurrentRound = previousRound;
    }

    private void Next()
    {
        _jsonService.SaveAsync(this, "game.json");
        var nextRound = Rounds.SingleOrDefault(r => r.Order == _currentRound.Order + 1);
        if (nextRound != null)
            CurrentRound = nextRound;
    }

    private void Close()
    {
        Action<bool> Callback1() => result =>
        {
            if (!result) return;

            Game = null;
            Rounds = new List<IRoundViewModel>();
            CurrentRound = null;
            Navigate<ISettingsPage>();
        };

        Action<bool> Callback2() => result =>
        {
            if (result) Application.Current?.Quit();
        };

        _alertService.ShowActionSheet("What Next?", "New Game", Callback1(), "Quit Game", Callback2());
    }

    private void DiscardPickup()
    {
        var random = new Random(Environment.TickCount);


        var shuffledWeightedList = GetShuffledWeightedList();
        var amount = shuffledWeightedList[random.Next(shuffledWeightedList.Count)];
        _alertService.ShowAlert("Discard Pickup", $"Pickup {amount} cards from the discard pile.");
    }

    private IList<int> GetShuffledWeightedList()
    {
        const int defaultPickup = 8;
        var weightedList = new List<int>();

        for (var i = MinDiscardPickup; i < MaxDiscardPickup; i++)
        {
            var weight = Math.Abs(defaultPickup - (defaultPickup - i));

            for (var j = 1; j < weight; j++) weightedList.Add(i);
        }

        return weightedList.Shuffle();
    }
}