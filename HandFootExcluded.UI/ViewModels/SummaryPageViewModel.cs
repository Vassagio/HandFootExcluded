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
    int FontSize {get; }
}

internal sealed record SummaryLine(string RoundOrder, string Description, string Player1Score, string Player2Score, string Player3Score, string Player4Score, string Player5Score, bool IsBold, int FontSize) : ISummaryLine
{
    private const string ZERO = "0";
    public static readonly SummaryLine EMPTY = new SummaryLine(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, 12);
}

public interface ISummaryPageViewModel : IViewModel
{
    IEnumerable<ISummaryLine> SummaryLines { get; }
    string Player1Name {get;}
    string Player2Name { get; }
    string Player3Name { get; }
    string Player4Name { get; }
    string Player5Name { get; }
    ICommand CloseCommand { get; }
}

internal sealed class SummaryPageViewModel : ViewModelBase, ISummaryPageViewModel
{
    private const string ZERO = "0";
    private IEnumerable<ISummaryLine> _summaryLines;
    private ICommand _closeCommand;
    private string _player1Name;
    private string _player2Name;
    private string _player3Name;
    private string _player4Name;
    private string _player5Name;

    public IEnumerable<ISummaryLine> SummaryLines { get => _summaryLines; set => SetProperty(ref _summaryLines, value); }
    public string Player1Name { get => _player1Name; set => SetProperty(ref _player1Name, value);}
    public string Player2Name { get => _player2Name; set => SetProperty(ref _player2Name, value); }
    public string Player3Name { get => _player3Name; set => SetProperty(ref _player3Name, value); }
    public string Player4Name { get => _player4Name; set => SetProperty(ref _player4Name, value); }
    public string Player5Name { get => _player5Name; set => SetProperty(ref _player5Name, value); }
    public ICommand CloseCommand => _closeCommand ?? new Command(Close);

    public SummaryPageViewModel() { EventAggregator.Instance.RegisterHandler<ScoreLinesChangedEvent>(OnScoreLinesChanged); }

    private void OnScoreLinesChanged(ScoreLinesChangedEvent scoreLinesChangedEvent)
    {
        var scoreLines = scoreLinesChangedEvent.ScoreLines.ToList();
        var roundScoreLines = scoreLines.OfType<IRoundScoreLine>().ToList();
        var playerOrder = scoreLines.OfType<IGrandTotalScoreLine>().OrderByDescending(s => s.Value).ToList();

        SetPlayerNames(playerOrder);

        var roundOrder = string.Empty;
        var description = string.Empty;
        var player1Score = string.Empty;
        var player2Score = string.Empty;
        var player3Score = string.Empty;
        var player4Score = string.Empty;
        var player5Score = string.Empty;
        var isBold = false;
        var fontSize = 12;
        var summaryLines = new List<ISummaryLine>();
        for (var roundIndex = 1; roundIndex <= 5; roundIndex++)
        {
            roundOrder = roundIndex.ToString();
            summaryLines.Add(new SummaryLine(roundOrder, $"Round {roundOrder}", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true, 20));
            for (var scoreIndex = 1; scoreIndex <= 5; scoreIndex++)
            {
                var scores = roundScoreLines.Where(l => l.RoundOrder == roundIndex && l.Order == scoreIndex).ToList();
                var score = scores.First();
                description = score.Name;
                isBold = score.IsBold;
                fontSize = score.FontSize;
                for (var playerIndex = 1; playerIndex <= playerOrder.Count(); playerIndex++)
                {
                    switch (playerIndex)
                    {
                        case 1: player1Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO; break;
                        case 2: player2Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO; break;
                        case 3: player3Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO; break;
                        case 4: player4Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO; break;
                        case 5: player5Score = scores.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO; break;
                        default: continue;
                    }
                }

                ISummaryLine summaryLine;
                if (score is IRoundTotalScoreLine || score is ICumulativeScoreLine)
                    summaryLine = new SummaryLine(roundOrder.ToString(), $"{description}", player1Score, player2Score, player3Score, player4Score, player5Score, isBold, fontSize);
                else
                    summaryLine = new SummaryLine(roundOrder.ToString(), $"\t\t{description}", player1Score, player2Score, player3Score, player4Score, player5Score, isBold, fontSize);
                summaryLines.Add(summaryLine);
            }
            summaryLines.Add(SummaryLine.EMPTY);
        }

        var grandTotal1Score = string.Empty;
        var grandTotal2Score = string.Empty;
        var grandTotal3Score = string.Empty;
        var grandTotal4Score = string.Empty;
        var grandTotal5Score = string.Empty;
        for (var playerIndex = 1; playerIndex <= playerOrder.Count(); playerIndex++)
        {
            switch (playerIndex)
            {
                case 1:
                    grandTotal1Score = playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO;
                    break;
                case 2:
                    grandTotal2Score = playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO;
                    break;
                case 3:
                    grandTotal3Score = playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO;
                    break;
                case 4:
                    grandTotal4Score = playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO;
                    break;
                case 5:
                    grandTotal5Score = playerOrder.FirstOrDefault(s => s.Initials == playerOrder[playerIndex - 1].Initials)?.Value.ToString() ?? ZERO;
                    break;
                default: continue;
            }
        }

        summaryLines.Add(SummaryLine.EMPTY);
        var grandTotalLine = new SummaryLine(string.Empty, "Total", grandTotal1Score, grandTotal2Score, grandTotal3Score, grandTotal4Score, grandTotal5Score, true, 20);
        summaryLines.Add(grandTotalLine);
        SummaryLines = summaryLines;
    }

    private void SetPlayerNames(List<IGrandTotalScoreLine> playerOrder)
    {
        for (var playerIndex = 1; playerIndex <= playerOrder.Count(); playerIndex++)
        {
            switch (playerIndex)
            {
                case 1:
                    Player1Name = playerOrder[playerIndex - 1].Initials;
                    break;
                case 2:
                    Player2Name = playerOrder[playerIndex - 1].Initials;
                    break;
                case 3:
                    Player3Name = playerOrder[playerIndex - 1].Initials;
                    break;
                case 4:
                    Player4Name = playerOrder[playerIndex - 1].Initials;
                    break;
                case 5:
                    Player5Name = playerOrder[playerIndex - 1].Initials;
                    break;
                default: continue;
            }
        }
    }

    private void Close() { Navigate<IGamePage>(); }
}