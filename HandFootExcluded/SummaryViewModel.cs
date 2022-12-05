using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.ScoreLines;

namespace HandFootExcluded;

public interface ISummaryViewModel
{
    IGame Game { get; }
    Command NewCommand { get; }
    Command CloseCommand { get; }
}

public record Line(string RowHeader, string Player1Value, string Player2Value, string Player3Value, string Player4Value, string Player5Value, Color BackgroundColor, bool IsBold);

internal sealed class SummaryViewModel : BindableItem, ISummaryViewModel
{
    

    private readonly IScoringService _scoringService;

    private IGame _game;
    private IEnumerable<Line> _lines = Enumerable.Empty<Line>();
    

    private Command _newCommand;
    private Command _closeCommand;

    public IGame Game { get => _game; set => SetProperty(ref _game, value); }
    public IEnumerable<Line> Lines {get => _lines; set => SetProperty(ref _lines, value);}

    public Command NewCommand => _newCommand ?? new Command(New);
    public Command CloseCommand => _closeCommand ?? new Command(Close);

    public SummaryViewModel(IScoringService scoringService)
    {
        _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));

        EventAggregator.Instance.RegisterHandler<GameFinishedEvent>(OnGameFinished);
    }

    private void OnGameFinished(GameFinishedEvent gameFinishedEvent)
    {
        Game = gameFinishedEvent.Game;
        var scoreLines = _scoringService.GetSummary(_game).ToList();
        var roundLines = scoreLines.OfType<IRoundScoreLine>().OrderBy(sl => sl.Round.Index).ThenBy(sl => sl.DisplayOrder).ToList();
        var gameLines = scoreLines.OfType<GameTotalScore>().ToList();
        var players = gameLines.OrderByDescending(sl => sl.Value).Select(sl => sl.Player).Distinct().ToList();

        var lines = new List<Line>();
        
        var playerLine = new Line("Players", players[0].FirstName, players[1].FirstName, players[2].FirstName, players[3].FirstName, players[4].FirstName, Colors.DarkGray, true);
        lines.Add(playerLine);

        for (int i = 1; i <= 5; i++)
        {
            var bonusLine = GetLine<BonusScore>(i, roundLines, players, Colors.White);
            lines.Add(bonusLine);

            var topLine = GetLine<TopScore>(i, roundLines, players, Colors.White);
            lines.Add(topLine);

            var bottomLine = GetLine<BottomScore>(i, roundLines, players, Colors.White);
            lines.Add(bottomLine);

            var roundLine = GetLine<RoundTotalScore>(i, roundLines, players, Colors.LightGray, true);
            lines.Add(roundLine);
        }

        var gameLine = GetLine<GameTotalScore>(scoreLines, players);
        lines.Add(gameLine);

        Lines = lines;
    }

    private Line GetLine<T>(int roundIndex, IReadOnlyList<IRoundScoreLine> roundLines, IReadOnlyList<IPlayer> players, Color backgroundColor, bool isBold = false) where T : IRoundScoreLine
    {
        var lines = roundLines.Where(rl => rl is T score && score.Round.Index == roundIndex).ToList();
        var values = GetValues(lines, players);
        var rowHeader = typeof(T) == typeof(RoundTotalScore) ? $"\t\t{values.Name} {roundIndex}" : $"\t\t\t\t{values.Name}";
        var line = new Line(rowHeader, values.Player1, values.Player2, values.Player3, values.Player4, values.Player5, backgroundColor, isBold);
        return line;
    }

    private Line GetLine<T>(IReadOnlyList<IScoreLine> roundLines, IReadOnlyList<IPlayer> players) where T : IScoreLine
    {
        var lines = roundLines.Where(rl => rl is T score).ToList();
        var values = GetValues(lines, players);
        var line = new Line(values.Name, values.Player1, values.Player2, values.Player3, values.Player4, values.Player5, Colors.DarkGray, true);
        return line;
    }

    private (string Name, string Player1, string Player2, string Player3, string Player4, string Player5) GetValues(IReadOnlyList<IScoreLine> roundScoreLines, IReadOnlyList<IPlayer> players)
    {
        var name = roundScoreLines.First().DisplayName;
        var player1Value = roundScoreLines.Single(bl => bl.Player.Equals(players[0])).Value.ToString();
        var player2Value = roundScoreLines.Single(bl => bl.Player.Equals(players[1])).Value.ToString();
        var player3Value = roundScoreLines.Single(bl => bl.Player.Equals(players[2])).Value.ToString();
        var player4Value = roundScoreLines.Single(bl => bl.Player.Equals(players[3])).Value.ToString();
        var player5Value = roundScoreLines.Single(bl => bl.Player.Equals(players[4])).Value.ToString();

        return (name, player1Value, player2Value, player3Value, player4Value, player5Value);
    }

    private static void New() => EventAggregator.Instance.SendMessage(NewGameEvent.Instance);
    private static void Close() => Application.Current?.Quit();
}