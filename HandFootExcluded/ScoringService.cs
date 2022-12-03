namespace HandFootExcluded;

internal interface IScoringService
{
    IPlayerScore CalculateFor(IGame game, IPlayer player);
}

internal sealed class ScoringService : IScoringService
{
    private static readonly int BONUS = 100;

    public IPlayerScore CalculateFor(IGame game, IPlayer player)
    {
        var score = 0;
        foreach (var round in game)
        {
            if (round.StartingPlayer.Equals(player) && round.StartingPlayerBonus) score += BONUS;
            if (round.StartingPartner.Equals(player) && round.StartingPartnerBonus) score += BONUS;
            if (round.OpposingPlayer.Equals(player) && round.OpposingPlayerBonus) score += BONUS;
            if (round.OpposingPartner.Equals(player) && round.OpposingPartnerBonus) score += BONUS;

            if (round.StartingTeam.Contains(player)) score += round.StartingTeam.Score;
            if (round.OpposingTeam.Contains(player)) score += round.OpposingTeam.Score;
        }

        return new PlayerScore(player, score);
    }
}