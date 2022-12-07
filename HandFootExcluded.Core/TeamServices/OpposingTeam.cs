using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface IOpposingTeam : ITeam<IOpposingPlayer, IOpposingPartner>
{
    new IOpposingPlayer Player { get; }
    new IOpposingPartner Partner { get; }
}

internal sealed class OpposingTeam : Team<IOpposingPlayer, IOpposingPartner>, IOpposingTeam
{
    public OpposingTeam(IOpposingPlayer player, IOpposingPartner partner) : base(player, partner) { }
}