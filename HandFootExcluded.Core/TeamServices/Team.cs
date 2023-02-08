using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface ITeam
{
    string Name { get; }
    IPlayers Players { get; }
    IPositionalPlayer Player { get; }
    IPositionalPlayer Partner { get; }
}

public interface ITeam<out TPlayer, out TPartner> : ITeam where TPlayer : IPositionalPlayer
                                                          where TPartner : IPositionalPlayer
{
    new TPlayer Player { get; }
    new TPartner Partner { get; }
}

internal sealed partial class TeamBuilder : BuilderBase<TeamBuilder, ITeam>, ITeamBuilder, ITeamBuilderPartner, ITeamBuilderBuild
{
    private abstract class Team<TPlayer, TPartner> : ITeam<TPlayer, TPartner> where TPlayer : IPositionalPlayer
                                                                              where TPartner : IPositionalPlayer
    {
        protected Team(TPlayer player, TPartner partner)
        {
            Player = player;
            Partner = partner;

            Players = new Players(player, partner);
        }
        public abstract string Name { get; }
        public IPlayers Players { get; }
        public TPlayer Player { get; }
        public TPartner Partner { get; }

        IPositionalPlayer ITeam.Partner => Partner;
        IPositionalPlayer ITeam.Player => Player;
    }
}