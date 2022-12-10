using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;
using System.Windows.Input;
using HandFootExcluded.UI.Services;

namespace HandFootExcluded.UI.ViewModels;

public interface IGamePageViewModel : IViewModel
{
    IGame Game { get; }
    IEnumerable<IRoundViewModel> Rounds { get; }
    IRoundViewModel CurrentRound {get;}
    ITotalScoreViewModel TotalScore { get; }

    ICommand SummaryCommand { get; }
    ICommand PreviousCommand { get; }
    ICommand NextCommand { get; }
    ICommand CloseCommand { get; }
}

internal sealed class GamePageViewModel : ViewModelBase, IGamePageViewModel
{
    private readonly IGameService _gameService;
    private readonly IScoringService _scoringService;
    private readonly IAlertService _alertService;

    private IGame _game;
    private IEnumerable<IRoundViewModel> _rounds;
    private IRoundViewModel _currentRound;
    private ITotalScoreViewModel _totalScore;

    private ICommand _summaryCommand;
    private ICommand _previousCommand;
    private ICommand _nextCommand;
    private ICommand _closeCommand;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IEnumerable<IRoundViewModel> Rounds { get => _rounds; set => SetProperty(ref _rounds, value); }
    public IRoundViewModel CurrentRound {get =>_currentRound; set => SetProperty(ref _currentRound, value);}
    public ITotalScoreViewModel TotalScore { get => _totalScore; set => SetProperty(ref _totalScore, value); }

    public ICommand SummaryCommand => _summaryCommand ?? new Command(Summary);
    public ICommand PreviousCommand => _summaryCommand ?? new Command(Previous);
    public ICommand NextCommand => _summaryCommand ?? new Command(Next);
    public ICommand CloseCommand => _summaryCommand ?? new Command(Close);

    public GamePageViewModel(IGameService gameService, IScoringService scoringService, IAlertService alertService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
        _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));

        _totalScore = new TotalScoreViewModel(new ScoreLines());

        EventAggregator.Instance.RegisterHandler<PlayersChangedEvent>(OnPlayersChanged);
        EventAggregator.Instance.RegisterHandler<ScoreChangedEvent>(OnScoreChanged);
    }

    private async void OnScoreChanged(ScoreChangedEvent scoreChangedEvent)
    {
        var scoreLines = await _scoringService.Score(Game, Rounds);
        TotalScore = new TotalScoreViewModel(scoreLines.GetGrandTotalLines());
    }

    private void OnPlayersChanged(PlayersChangedEvent playersChangedEvent)
    {
        Game ??= _gameService.CreateGame(playersChangedEvent.Players);
        if (Rounds == null || !Rounds.Any())
        {
            var rounds = Game.Rounds.Select(ToViewModel).ToList();
            Rounds = rounds;
            CurrentRound = Rounds.FirstOrDefault();
        }

        OnScoreChanged(new ScoreChangedEvent());
    }

    private IRoundViewModel ToViewModel(IRound round) =>
        new RoundViewModel()
        {
            Order = round.Order,
            OpenAmount = round.OpenAmount,
            StartingTeam = ToViewModel(round.Teams.Find<IStartingTeam>()),
            OpposingTeam = ToViewModel(round.Teams.Find<IOpposingTeam>()),
            ExcludedPlayerInitials = round.Players?.Find<IExcludedPlayer>()?.Initials ?? string.Empty
        };

    private ITeamViewModel ToViewModel(ITeam team) =>
        new TeamViewModel()
        {
            TeamName = team.Name,
            PlayerInitials = team.Player.Initials,
            PartnerInitials = team.Partner.Initials
        };

    private void Summary()
    {
        var scoreLines = _scoringService.Score(Game, Rounds).Result;
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
            CurrentRound = nextRound;
    }

    private void Close()
    {
        Action<bool> Callback1() => result =>
        {
            if (result) 
            {
                Game = null;
                Rounds = new List<IRoundViewModel>();
                CurrentRound = null;
                Navigate<ISettingsPage>();
            }
        };

        Action<bool> Callback2() => result =>
        {
            if (result) Application.Current?.Quit();
        };

        _alertService.ShowActionSheet("What Next?", "New Game", Callback1(), "Quit Game", Callback2());
    }
}