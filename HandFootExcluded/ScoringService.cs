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
        var bonusCount = 0;
        foreach (var round in game)
        {
            if (round.StartingPlayer.Equals(player) && round.StartingPlayerBonus) AddBonus(ref score, ref bonusCount);            
            if (round.StartingPartner.Equals(player) && round.StartingPartnerBonus) AddBonus(ref score, ref bonusCount);            
            if (round.OpposingPlayer.Equals(player) && round.OpposingPlayerBonus) AddBonus(ref score, ref bonusCount);            
            if (round.OpposingPartner.Equals(player) && round.OpposingPartnerBonus) AddBonus(ref score, ref bonusCount);
            
            if (round.StartingTeam.Contains(player)) score += round.StartingTeam.Score;
            if (round.OpposingTeam.Contains(player)) score += round.OpposingTeam.Score;
        }

        return new PlayerScore(player, score, bonusCount);
    }
    
    private void AddBonus(ref int score, ref int bonusCount) 
    {
        score += BONUS;
        bonusCount++;
    }
}