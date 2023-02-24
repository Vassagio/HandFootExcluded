using System.Windows.Input;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services;
using HandFootExcluded.UI.Services.GameHistoryServices;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface IGamePageViewModel : IViewModel
{
    IGame Game { get; }
    IEnumerable<IRoundViewModel> Rounds { get; }
    IRoundViewModel CurrentRound { get; }
    ITotalScoreViewModel TotalScore { get; }
    int BonusAmount { get; }

    ICommand SummaryCommand { get; }
    ICommand PreviousCommand { get; }
    ICommand NextCommand { get; }
    ICommand CloseCommand { get; }
    ICommand DiscardPickupCommand { get; }
}

internal sealed class GamePageViewModel : ViewModelBase, IGamePageViewModel
{
    private readonly IAlertService _alertService;
    private readonly IGameHistoryService _gameHistoryService;
    private readonly IGameService _gameService;
    private readonly IScoringService _scoringService;
    private int _bonusAmount;
    private ICommand _closeCommand;
    private IRoundViewModel _currentRound;
    private ICommand _discardPickupCommand;

    private IGame _game;
    private int _maxDiscardPickup;
    private int _minDiscardPickup;
    private ICommand _nextCommand;
    private ICommand _previousCommand;
    private IEnumerable<IRoundViewModel> _rounds;

    private ICommand _summaryCommand;
    private ITotalScoreViewModel _totalScore;
    public int MinDiscardPickup { get => _minDiscardPickup; set => SetProperty(ref _minDiscardPickup, value); }
    public int MaxDiscardPickup { get => _maxDiscardPickup; set => SetProperty(ref _maxDiscardPickup, value); }

    public GamePageViewModel(IGameService gameService, IScoringService scoringService, IGameHistoryService gameHistoryService, IAlertService alertService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
        _gameHistoryService = gameHistoryService ?? throw new ArgumentNullException(nameof(gameHistoryService));
        _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));

        _totalScore = new TotalScoreViewModel(new ScoreLines());

        EventAggregator.Instance.RegisterHandler<SettingsChangedEvent>(OnSettingsChanged);
        EventAggregator.Instance.RegisterHandler<ScoreChangedEvent>(OnScoreChanged);
    }

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IEnumerable<IRoundViewModel> Rounds { get => _rounds; set => SetProperty(ref _rounds, value); }
    public IRoundViewModel CurrentRound { get => _currentRound; set => SetProperty(ref _currentRound, value); }
    public ITotalScoreViewModel TotalScore { get => _totalScore; set => SetProperty(ref _totalScore, value); }
    public int BonusAmount { get => _bonusAmount; set => SetProperty(ref _bonusAmount, value); }

    public ICommand SummaryCommand => SetCommand(ref _summaryCommand, Summary);
    public ICommand PreviousCommand => SetCommand(ref _previousCommand, Previous);
    public ICommand NextCommand => SetCommand(ref _nextCommand, Next);
    public ICommand CloseCommand => SetCommand(ref _closeCommand, Close);
    public ICommand DiscardPickupCommand => SetCommand(ref _discardPickupCommand, DiscardPickup);

    private async void OnScoreChanged(ScoreChangedEvent scoreChangedEvent)
    {
        var scoreLines = await _scoringService.Score(Game, Rounds, BonusAmount);
        TotalScore = new TotalScoreViewModel(scoreLines.GetGrandTotalLines());
    }

    private void OnSettingsChanged(SettingsChangedEvent settingsChangedEvent)
    {
        BonusAmount = settingsChangedEvent.BonusAmount;
        MinDiscardPickup = settingsChangedEvent.MinDiscardPickup;
        MaxDiscardPickup = settingsChangedEvent.MaxDiscardPickup;
        Game ??= _gameService.CreateGame(settingsChangedEvent.Players, settingsChangedEvent.RoundOpeningAmounts);

        if (Rounds == null || !Rounds.Any())
        {
            var rounds = Game.Rounds.Select(ToViewModel)
                             .ToList();
            Rounds = rounds;
            CurrentRound = Rounds.FirstOrDefault();
        }

        OnScoreChanged(new ScoreChangedEvent());
    }

    private static IRoundViewModel ToViewModel(IRound round) =>
        new RoundViewModel
        {
            Order = round.Order,
            OpenAmount = round.OpenAmount,
            StartingTeam = ToViewModel(round.Teams.Find<IStartingTeam>()),
            OpposingTeam = ToViewModel(round.Teams.Find<IOpposingTeam>()),
            ExcludedPlayerInitials = round.Players?.Find<IExcludedPlayer>()
                                         ?.Initials ?? string.Empty
        };

    private static ITeamViewModel ToViewModel(ITeam team) =>
        new TeamViewModel
        {
            TeamName = team.Name,
            PlayerInitials = team.Player.Initials,
            PartnerInitials = team.Partner.Initials
        };

    private void Summary()
    {
        var scoreLines = _scoringService.Score(Game, Rounds, BonusAmount)
                                        .Result;
        EventAggregator.Instance.PostMessage(new ScoreLinesChangedEvent(scoreLines));
        Navigate<ISummaryPage>();
    }

    private void Previous()
    {
        var previousRound = Rounds.SingleOrDefault(r => r.Order == _currentRound.Order - 1);
        if (previousRound != null)
            CurrentRound = previousRound;
    }

    private void Next()
    {
        var nextRound = Rounds.SingleOrDefault(r => r.Order == _currentRound.Order + 1);

        if (nextRound != null)
        {
            CurrentRound = nextRound;
        }
        else
        {
            _gameHistoryService.Save(_game, _rounds);
            Summary();
        }
    }

    private void Close()
    {
        Action<bool> NewGameCallback() =>
            result =>
            {
                if (!result) return;

                Game = null;
                Rounds = new List<IRoundViewModel>();
                CurrentRound = null;
                Navigate<IMainPage>();
            };

        Action<bool> QuitGameCallback() =>
            result =>
            {
                if (result) Application.Current?.Quit();
            };

        _alertService.ShowActionSheet("What Next?", "New Game", NewGameCallback(), "Quit Game", QuitGameCallback());
    }

    private void DiscardPickup()
    {
        var random = new Random(Environment.TickCount);
        var amount = random.NextInt64(MinDiscardPickup, MaxDiscardPickup);
        _alertService.ShowAlert("Discard Pickup", $"Pickup {amount} cards from the discard pile.");
    }
}