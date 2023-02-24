using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.GameHistoryServices;

public interface IGameHistory
{
    DateTime DatePlayed { get; }
    int BonusAmount { get; }
    int MinDiscardPickup { get; }
    int MaxDiscardPickup { get; }
    int RoundOpening1 { get; }
    int RoundOpening2 { get; }
    int RoundOpening3 { get; }
    int RoundOpening4 { get; }
    int RoundOpening5 { get; }
    int Player1Score { get; }
    string Player1Name { get; }
    int Player2Score { get; }
    string Player2Name { get; }
    int Player3Score { get; }
    string Player3Name { get; }
    int Player4Score { get; }
    string Player4Name { get; }
    int Player5Score { get; }
    string Player5Name { get; }
}

internal sealed class GameHistory : IGameHistory
{
    private readonly string _player1Name;

    private readonly int _player1Score;
    private readonly string _player2Name;
    private readonly int _player2Score;
    private readonly string _player3Name;
    private readonly int _player3Score;
    private readonly string _player4Name;
    private readonly int _player4Score;
    private readonly string _player5Name;
    private readonly int _player5Score;

    public DateTime DatePlayed { get; }
    public int BonusAmount { get; }
    public int MinDiscardPickup { get; }
    public int MaxDiscardPickup { get; }
    public int RoundOpening1 { get; }
    public int RoundOpening2 { get; }
    public int RoundOpening3 { get; }
    public int RoundOpening4 { get; }
    public int RoundOpening5 { get; }

    public int Player1Score => _player1Score;
    public string Player1Name => _player1Name;
    public int Player2Score => _player2Score;
    public string Player2Name => _player2Name;
    public int Player3Score => _player3Score;
    public string Player3Name => _player3Name;
    public int Player4Score => _player4Score;
    public string Player4Name => _player4Name;
    public int Player5Score => _player5Score;
    public string Player5Name => _player5Name;

    public GameHistory(DateTime datePlayed, IScoreLines score, ISettingsPageViewModel settings)
    {
        DatePlayed = datePlayed;
        BonusAmount = settings.BonusAmount;
        MinDiscardPickup = settings.MinDiscardPickup;
        MaxDiscardPickup = settings.MaxDiscardPickup;
        RoundOpening1 = settings.RoundOpening1;
        RoundOpening2 = settings.RoundOpening2;
        RoundOpening3 = settings.RoundOpening3;
        RoundOpening4 = settings.RoundOpening4;
        RoundOpening5 = settings.RoundOpening5;

        var players = score.GetPlayers()
                           .ToList();
        var grandTotals = score.GetGrandTotalLines();

        SetPlayer(players, 0, grandTotals, out _player1Name, out _player1Score);
        SetPlayer(players, 1, grandTotals, out _player2Name, out _player2Score);
        SetPlayer(players, 2, grandTotals, out _player3Name, out _player3Score);
        SetPlayer(players, 3, grandTotals, out _player4Name, out _player4Score);
        SetPlayer(players, 4, grandTotals, out _player5Name, out _player5Score);
    }

    private static void SetPlayer(IReadOnlyList<string> players, int playerIndex, IScoreLines grandTotals, out string playerName, out int playerScore)
    {
        var player = players[playerIndex];
        playerName = player;
        playerScore = grandTotals.FirstOrDefault(gt => gt.Initials == player)
                                ?.Value ?? 0;
    }
}