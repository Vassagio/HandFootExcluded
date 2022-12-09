using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI.ViewModels;

public interface IGamePageViewModel : IViewModel
{
    IGame Game { get; }
    IEnumerable<IRoundViewModel> Rounds { get; }
    ITotalScoreViewModel TotalScore { get; }
}

internal sealed class GamePageViewModel : ViewModelBase, IGamePageViewModel
{
    private readonly IGameService _gameService;
    private readonly IScoringService _scoringService;

    private IGame _game;
    private IEnumerable<IRoundViewModel> _rounds;
    private ITotalScoreViewModel _totalScore;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IEnumerable<IRoundViewModel> Rounds { get => _rounds; set => SetProperty(ref _rounds, value); }
    public ITotalScoreViewModel TotalScore { get => _totalScore; set => SetProperty(ref _totalScore, value); }

    public GamePageViewModel(IGameService gameService, IScoringService scoringService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));

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
}