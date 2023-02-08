using HandFootExcluded.Common;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface ITeamBuilder : IBuilder
{
    ITeamBuilderPartner WithPlayer(IStartingPlayer player);
    ITeamBuilderPartner WithPlayer(IOpposingPlayer player);
}

public interface ITeamBuilderPartner : IBuilder
{
    ITeamBuilderBuild WithPartner(IStartingPartner partner);
    ITeamBuilderBuild WithPartner(IOpposingPartner partner);
}

public interface ITeamBuilderBuild : IBuilder<ITeam>
{
}

internal sealed partial class TeamBuilder : BuilderBase<TeamBuilder, ITeam>, ITeamBuilder, ITeamBuilderPartner, ITeamBuilderBuild
{
    private IPositionalPlayer _partner;
    private IPositionalPlayer _player;
    public ITeamBuilderPartner WithPlayer(IStartingPlayer player) => SetProperty(ref _player, player);
    public ITeamBuilderPartner WithPlayer(IOpposingPlayer player) => SetProperty(ref _player, player);
    public ITeamBuilderBuild WithPartner(IStartingPartner partner) => SetProperty(ref _partner, partner);
    public ITeamBuilderBuild WithPartner(IOpposingPartner partner) => SetProperty(ref _partner, partner);

    protected override ITeam BuildInternal() =>
        _player switch
        {
            IStartingPlayer startingPlayer when _partner is IStartingPartner startingPartner => new StartingTeam(startingPlayer, startingPartner),
            IOpposingPlayer opposingPlayer when _partner is IOpposingPartner opposingPartner => new OpposingTeam(opposingPlayer, opposingPartner),
            _                                                                                => UnknownTeam.Instance
        };
}