using System.Windows.Input;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface ISummaryLine
{
    string RoundOrder { get; }
    string Description { get; }
    string Player1Score { get; }
    string Player2Score { get; }
    string Player3Score { get; }
    string Player4Score { get; }
    string Player5Score { get; }
    bool IsBold { get; }
    int FontSize { get; }
}

internal sealed record SummaryLine(string RoundOrder, string Description, string Player1Score, string Player2Score, string Player3Score, string Player4Score, string Player5Score, bool IsBold, int FontSize) : ISummaryLine
{
    public static readonly SummaryLine Empty = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, 12);
}

public interface ISummaryPageViewModel : IViewModel
{
    IEnumerable<ISummaryLine> SummaryLines { get; }
    string Player1Name { get; }
    string Player2Name { get; }
    string Player3Name { get; }
    string Player4Name { get; }
    string Player5Name { get; }
    ICommand CloseCommand { get; }
}

internal sealed class SummaryPageViewModel : ViewModelBase, ISummaryPageViewModel
{
    private const string Zero = "0";
    private ICommand _closeCommand;
    private string _player1Name;
    private string _player2Name;
    private string _player3Name;
    private string _player4Name;
    private string _player5Name;
    private IEnumerable<ISummaryLine> _summaryLines;

    public SummaryPageViewModel() => EventAggregator.Instance.RegisterHandler<ScoreLinesChangedEvent>(OnScoreLinesChanged);

    public IEnumerable<ISummaryLine> SummaryLines { get => _summaryLines; set => SetProperty(ref _summaryLines, value); }
    public string Player1Name { get => _player1Name; set => SetProperty(ref _player1Name, value); }
    public string Player2Name { get => _player2Name; set => SetProperty(ref _player2Name, value); }
    public string Player3Name { get => _player3Name; set => SetProperty(ref _player3Name, value); }
    public string Player4Name { get => _player4Name; set => SetProperty(ref _player4Name, value); }
    public string Player5Name { get => _player5Name; set => SetProperty(ref _player5Name, value); }
    public ICommand CloseCommand => SetCommand(ref _closeCommand, Close);

    private void OnScoreLinesChanged(ScoreLinesChangedEvent scoreLinesChangedEvent)
    {
        var scoreLines = scoreLinesChangedEvent.ScoreLines.ToList();
        var roundScoreLines = scoreLines.OfType<IRoundScoreLine>()
                                        .ToList();
        var playerOrder = scoreLines.OfType<IGrandTotalScoreLine>()
                                    .OrderByDescending(s => s.Value)
                                    .ToList();

        SetPlayerNames(playerOrder);

        var player1Score = string.Empty;
        var player2Score = string.Empty;
        var player3Score = string.Empty;
        var player4Score = string.Empty;
        var player5Score = string.Empty;
        var summaryLines = new List<ISummaryLine>();

        for (var roundIndex = 1; roundIndex <= 5; roundIndex++)
        {
            var roundOrder = roundIndex.ToString();
            summaryLines.Add(new SummaryLine(roundOrder, $"Round {roundOrder}", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true, 20));

            for (var scoreIndex = 1; scoreIndex <= 5; scoreIndex++)
            {
                var scores = roundScoreLines.Where(l => l.RoundOrder == roundIndex && l.Order == scoreIndex)
                                            .ToList();
                var score = scores.First();
                var description = score.Name;
                var isBold = score.IsBold;
                var fontSize = score.FontSize;

                for (var playerIndex = 1; playerIndex <= playerOrder.Count; playerIndex++)
                    switch (playerIndex)
                    {
                        case 1:
                            player1Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                                                    .Initials)
                                                ?.Value.ToString() ?? Zero;
                            break;
                        case 2:
                            player2Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                                                    .Initials)
                                                ?.Value.ToString() ?? Zero;
                            break;
                        case 3:
                            player3Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                                                    .Initials)
                                                ?.Value.ToString() ?? Zero;
                            break;
                        case 4:
                            player4Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                                                    .Initials)
                                                ?.Value.ToString() ?? Zero;
                            break;
                        case 5:
                            player5Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                                                    .Initials)
                                                ?.Value.ToString() ?? Zero;
                            break;
                        default: continue;
                    }

                ISummaryLine summaryLine = score is IRoundTotalScoreLine or ICumulativeScoreLine ?
                    new SummaryLine(roundOrder, $"{description}", player1Score, player2Score, player3Score, player4Score, player5Score, isBold, fontSize) :
                    new SummaryLine(roundOrder, $"\t\t{description}", player1Score, player2Score, player3Score, player4Score, player5Score, isBold, fontSize);
                summaryLines.Add(summaryLine);
            }

            summaryLines.Add(SummaryLine.Empty);
        }

        var grandTotals = SetGrandTotals(playerOrder);

        summaryLines.Add(SummaryLine.Empty);
        var grandTotalLine = new SummaryLine(string.Empty, "Total", grandTotals.Score1, grandTotals.Score2, grandTotals.Score3, grandTotals.Score4, grandTotals.Score5, true, 20);
        summaryLines.Add(grandTotalLine);
        SummaryLines = summaryLines;
    }

    private static (string Score1, string Score2, string Score3, string Score4, string Score5) SetGrandTotals(IReadOnlyList<IGrandTotalScoreLine> playerOrder)
    {
        var score1 = string.Empty;
        var score2 = string.Empty;
        var score3 = string.Empty;
        var score4 = string.Empty;
        var score5 = string.Empty;

        string Score(int playerIndex) =>
            playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1]
                                          .Initials)
                      ?.Value.ToString() ?? Zero;

        for (var playerIndex = 1; playerIndex <= playerOrder.Count; playerIndex++)
            switch (playerIndex)
            {
                case 1:
                    score1 = Score(playerIndex);
                    break;
                case 2:
                    score2 = Score(playerIndex);
                    break;
                case 3:
                    score3 = Score(playerIndex);
                    break;
                case 4:
                    score4 = Score(playerIndex);
                    break;
                case 5:
                    score5 = Score(playerIndex);
                    break;
                default:
                    continue;
            }

        return (score1, score2, score3, score4, score5);
    }

    private void SetPlayerNames(IReadOnlyList<IGrandTotalScoreLine> playerOrder)
    {
        string Initials(int playerIndex) =>
            playerOrder[playerIndex - 1]
               .Initials;

        for (var playerIndex = 1; playerIndex <= playerOrder.Count; playerIndex++)
            switch (playerIndex)
            {
                case 1:
                    Player1Name = Initials(playerIndex);
                    break;
                case 2:
                    Player2Name = Initials(playerIndex);
                    break;
                case 3:
                    Player3Name = Initials(playerIndex);
                    break;
                case 4:
                    Player4Name = Initials(playerIndex);
                    break;
                case 5:
                    Player5Name = Initials(playerIndex);
                    break;
                default: continue;
            }
    }

    private static void Close() => Navigate<IGamePage>();
}