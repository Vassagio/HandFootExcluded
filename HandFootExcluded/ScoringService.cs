using HandFootExcluded.ScoreLines;

namespace HandFootExcluded;

internal interface IScoringService
{
    IPlayerScore CalculateFor(IGame game, IPlayer player);
    IEnumerable<IScoreLine> GetSummary(IGame game);
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

    public IEnumerable<IScoreLine> GetSummary(IGame game)
    {
        var scoreLines = new HashSet<IScoreLine>();
        foreach (var player in game.Players)
        {
            var gameScore = 0;
            foreach (var round in game)
            {
                var roundScore = 0;
                if (round.StartingTeam.Contains(player))
                {
                    if (round.StartingPlayer.Equals(player) && round.StartingPlayerBonus)
                    {
                        roundScore += BONUS;
                        scoreLines.Add(new BonusScore(player, round, BONUS));
                    }
                    else if (round.StartingPartner.Equals(player) && round.StartingPartnerBonus)
                    {
                        roundScore += BONUS;
                        scoreLines.Add(new BonusScore(player, round, BONUS));
                    }
                    else
                        scoreLines.Add(new BonusScore(player, round, 0));

                    scoreLines.Add(new TopScore(player, round, round.StartingTeam.TopScore));
                    scoreLines.Add(new BottomScore(player, round, round.StartingTeam.BottomScore));

                    roundScore += round.StartingTeam.TopScore + round.StartingTeam.BottomScore;
                    scoreLines.Add(new RoundTotalScore(player, round, roundScore));
                }
                else if (round.OpposingTeam.Contains(player))
                {
                    if (round.OpposingPlayer.Equals(player) && round.OpposingPlayerBonus)
                    {
                        roundScore += BONUS;
                        scoreLines.Add(new BonusScore(player, round, BONUS));
                    }
                    else if (round.OpposingPartner.Equals(player) && round.OpposingPartnerBonus)
                    {
                        roundScore += BONUS;
                        scoreLines.Add(new BonusScore(player, round, BONUS));
                    }
                    else
                        scoreLines.Add(new BonusScore(player, round, 0));

                    scoreLines.Add(new TopScore(player, round, round.OpposingTeam.TopScore));
                    scoreLines.Add(new BottomScore(player, round, round.OpposingTeam.BottomScore));

                    roundScore += round.OpposingTeam.TopScore + round.OpposingTeam.BottomScore;
                    scoreLines.Add(new RoundTotalScore(player, round, roundScore));
                }
                else
                {
                    scoreLines.Add(new BonusScore(player, round, 0));
                    scoreLines.Add(new TopScore(player, round, 0));
                    scoreLines.Add(new BottomScore(player, round, 0));
                    scoreLines.Add(new RoundTotalScore(player, round, 0));
                }

                gameScore += roundScore;
            }

            scoreLines.Add(new GameTotalScore(player, gameScore));
        }

        return scoreLines;
    }

    private static void AddBonus(ref int score, ref int bonusCount)
    {
        score += BONUS;
        bonusCount++;
    }
}