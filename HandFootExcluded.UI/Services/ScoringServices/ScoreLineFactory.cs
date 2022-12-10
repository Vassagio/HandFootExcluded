using HandFootExcluded.Core.GameServices;
using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLineFactory
{
    IScoreLines Create(IGame game, IEnumerable<IRoundViewModel> rounds);
}

internal sealed class ScoreLineFactory : IScoreLineFactory
{
    private IReadOnlyList<string> _players;
    private IReadOnlyList<int> _roundOrders;

    public IScoreLines Create(IGame game, IEnumerable<IRoundViewModel> rounds)
    {
        IScoreLines scoreLines = new ScoreLines();
        _players = game.OrderedPlayers.Select(p => p.Initials).ToList();
        _roundOrders = game.Rounds.Select(r => r.Order).ToList();

        foreach (var round in rounds.ToList())
            scoreLines.Add(CreatePlayerScores(round));

        scoreLines = CreateCumulativeScores(scoreLines);
        return CreateGrandTotalScore(scoreLines);
    }

    private static IScoreLines CreatePlayerScores(IRoundViewModel round)
    {
        var startingTeamScoreLines = CreateTeamScores(round, round.StartingTeam);
        var opposingTeamScoreLines = CreateTeamScores(round, round.OpposingTeam);
        var excludedPlayerScoreLines = CreateExcludedRoundTotal(round.ExcludedPlayerInitials, round);

        return new ScoreLines().Add(startingTeamScoreLines).Add(opposingTeamScoreLines).Add(excludedPlayerScoreLines);
    }

    private static IScoreLines CreateTeamScores(IRoundViewModel round, ITeamViewModel team)
    {
        var playerScoreLines = CreateRoundTotalScore(round, team, team.PlayerInitials, team.PlayerBonus);
        var partnerScoreLines = CreateRoundTotalScore(round, team, team.PartnerInitials, team.PartnerBonus);
        return new ScoreLines().Add(playerScoreLines).Add(partnerScoreLines);
    }

    private static IScoreLines CreateRoundTotalScore(IRoundViewModel round, ITeamViewModel team, string initials, bool hasBonus)
    {
        var bonus = new BonusScoreLine(round.Order, initials, hasBonus);
        var bottom = new BottomScoreLine(round.Order, initials, team.BottomScore);
        var top = new TopScoreLine(round.Order, initials, team.TopScore);
        var roundTotal = CreateRoundTotal(initials, round.Order, bonus, top, bottom);
        return new ScoreLines().Add(bonus).Add(top).Add(bottom).Add(roundTotal);
    }

    private static IRoundTotalScoreLine CreateRoundTotal(string initials, int roundOrder, IBonusScoreLine bonusScoreLine, ITopScoreLine topScoreLine, IBottomScoreLine bottomScoreLine) =>
        new RoundTotalScoreLine(roundOrder, initials, bonusScoreLine, topScoreLine, bottomScoreLine);

    private static IRoundTotalScoreLine CreateExcludedRoundTotal(string initials, IRoundViewModel round)
    {
        var bonus = new BonusScoreLine(round.Order, initials, false);
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