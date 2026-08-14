using Android.Net;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLineFactory
{
    IScoreLines Create(IGame game, IEnumerable<IRoundViewModel> rounds, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount);
}

internal sealed class ScoreLineFactory : IScoreLineFactory
{
    private IReadOnlyList<string> _players;
    private IReadOnlyList<int> _roundOrders;
    private IReadOnlyList<IRoundViewModel> _rounds;

    private record PlayerBonusCount(string Initials, int Round, int BonusCount);

    public IScoreLines Create(IGame game, IEnumerable<IRoundViewModel> rounds, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount)
    {
        IScoreLines scoreLines = new ScoreLines();
        _players = game.OrderedPlayers.Select(p => p.Initials).ToList();
        _roundOrders = game.Rounds.Select(r => r.Order).ToList();
         _rounds = rounds.ToList();

         var playerBonusCounts = GetPlayerBonusCounts(_players, _rounds).ToList();

        foreach (var round in _rounds)
            scoreLines.Add(CreatePlayerScores(round, individualBonusAmounts, teamBonusAmount, playerBonusCounts));

        scoreLines = CreateCumulativeScores(scoreLines);
        return CreateGrandTotalScore(scoreLines);
    }

    private static IEnumerable<PlayerBonusCount> GetPlayerBonusCounts(IReadOnlyList<string> players, IReadOnlyList<IRoundViewModel> rounds)
    {
        var playerBonuses = new List<PlayerBonusCount>();

        if (players == null || rounds == null)  
            return playerBonuses;

        foreach (var player in players)
            playerBonuses.Add(new PlayerBonusCount(player, 0, 0));
        
        foreach (var round in rounds)
        {
            if (round == null) continue;

            var startingTeam = round.StartingTeam;
            if (!string.IsNullOrEmpty(startingTeam?.PlayerInitials) && startingTeam.PlayerBonus)
                playerBonuses.Add(new PlayerBonusCount(startingTeam.PlayerInitials, round.Order, playerBonuses.Count(pb => pb.Initials == startingTeam.PlayerInitials)));
    
            if (!string.IsNullOrEmpty(startingTeam?.PartnerInitials) && startingTeam.PartnerBonus)
                playerBonuses.Add(new PlayerBonusCount(startingTeam.PartnerInitials, round.Order, playerBonuses.Count(pb => pb.Initials == startingTeam.PartnerInitials)));

            var opposingTeam = round.OpposingTeam;
            if (!string.IsNullOrEmpty(opposingTeam?.PlayerInitials) && opposingTeam.PlayerBonus)
                playerBonuses.Add(new PlayerBonusCount(opposingTeam.PlayerInitials, round.Order, playerBonuses.Count(pb => pb.Initials == opposingTeam.PlayerInitials)));

            if (!string.IsNullOrEmpty(opposingTeam?.PartnerInitials) && opposingTeam.PartnerBonus)
                playerBonuses.Add(new PlayerBonusCount(opposingTeam.PartnerInitials, round.Order, playerBonuses.Count(pb => pb.Initials == opposingTeam.PartnerInitials)));
        }

        return playerBonuses;
    }

    private static IScoreLines CreatePlayerScores(IRoundViewModel round, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount, IReadOnlyList<PlayerBonusCount> playerBonusCounts)
    {
        var startingTeamScoreLines = CreateTeamScores(round, round.StartingTeam, individualBonusAmounts, teamBonusAmount, playerBonusCounts);
        var opposingTeamScoreLines = CreateTeamScores(round, round.OpposingTeam, individualBonusAmounts, teamBonusAmount, playerBonusCounts);
        var excludedPlayerScoreLines = CreateExcludedRoundTotal(round.ExcludedPlayerInitials, round);

        return new ScoreLines().Add(startingTeamScoreLines).Add(opposingTeamScoreLines).Add(excludedPlayerScoreLines);
    }

    private static IScoreLines CreateTeamScores(IRoundViewModel round, ITeamViewModel team, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount, IReadOnlyList<PlayerBonusCount> playerBonusCounts)
    {
        var playerScoreLines = CreateRoundTotalScore(round, team, team.PlayerInitials, team.PlayerBonus, individualBonusAmounts, teamBonusAmount, playerBonusCounts);
        var partnerScoreLines = CreateRoundTotalScore(round, team, team.PartnerInitials, team.PartnerBonus, individualBonusAmounts, teamBonusAmount, playerBonusCounts);
        return new ScoreLines().Add(playerScoreLines).Add(partnerScoreLines);
    }

    private static IScoreLines CreateRoundTotalScore(IRoundViewModel round, ITeamViewModel team, string initials, bool hasBonus, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount, IReadOnlyList<PlayerBonusCount> playerBonusCounts)
    {
        var bonusCount = playerBonusCounts.FirstOrDefault(pbc => pbc.Initials == initials && pbc.Round == round.Order)?.BonusCount ?? 0;
        var hasTeamBonus = team.PlayerBonus && team.PartnerBonus;
        var bonusAmount =  hasTeamBonus ? individualBonusAmounts[bonusCount] + teamBonusAmount : individualBonusAmounts[bonusCount];
        var bonus = new BonusScoreLine(round.Order, initials, hasBonus, bonusAmount);
        var bottom = new BottomScoreLine(round.Order, initials, team.BottomScore);
        var top = new TopScoreLine(round.Order, initials, team.TopScore);
        var roundTotal = CreateRoundTotal(initials, round.Order, bonus, top, bottom);
        return new ScoreLines().Add(bonus).Add(top).Add(bottom).Add(roundTotal);
    }

    private static IRoundTotalScoreLine CreateRoundTotal(string initials, int roundOrder, IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) =>
        new RoundTotalScoreLine(roundOrder, initials, bonusScoreLine, topScoreLine, bottomScoreLine);

    private static IRoundTotalScoreLine CreateExcludedRoundTotal(string initials, IRoundViewModel round)
    {
        var bonus = new BonusScoreLine(round.Order, initials, false, 0);
        var bottom = new BottomScoreLine(round.Order, initials, 0);
        var top = new TopScoreLine(round.Order, initials, 0);
        return new RoundTotalScoreLine(round.Order, initials, bonus, top, bottom);
    }

    private IScoreLines CreateCumulativeScores(IScoreLines scoreLines)
    {
        foreach (var player in _players)
            foreach (var roundOrder in _roundOrders)
            {
                var roundScoreLines = GetRoundTotalScoreLines(scoreLines, roundOrder, player);
                scoreLines.Add(new CumulativeScoreLine(roundOrder, player, roundScoreLines));
            }
        return scoreLines;
    }

    private IScoreLines CreateGrandTotalScore(IScoreLines scoreLines)
    {
        foreach (var player in _players)
            scoreLines.Add(new GrandTotalScoreLine(player, GetCumulativeScoreLines(scoreLines, player)));
        return scoreLines;
    }

    private static IEnumerable<IRoundTotalScoreLine> GetRoundTotalScoreLines(IScoreLines scoreLines, int roundOrder, string player) =>
        scoreLines.OfType<IRoundTotalScoreLine>()
                  .Where(l => l.Initials.Equals(player) &&
                              l.RoundOrder <= roundOrder);

    private static ICumulativeScoreLine GetCumulativeScoreLines(IScoreLines scoreLines, string player) =>
        scoreLines.OfType<ICumulativeScoreLine>().Last(l => l.Initials.Equals(player));
}