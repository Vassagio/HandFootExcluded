using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.Core.TeamServices;

public interface ITeamBuilder : IBuilder
{
    ITeamBuilderPartner WithPlayer(IStartingPlayer player);
    ITeamBuilderPartner WithPlayer(IOpposingPlayer player);
}

public interface ITeamBuilderPartner : IBuilder
{
    ITeamBuilderBuild WithPartner(IStartingPlayer partner);
    ITeamBuilderBuild WithPartner(IOpposingPlayer partner);
}

public interface ITeamBuilderBuild : IBuilder<ITeam>
{

}


internal sealed class TeamBuilder : BuilderBase<TeamBuilder, ITeam>, ITeamBuilder, ITeamBuilderPartner, ITeamBuilderBuild
{
    private IPositionalPlayer _player;
    private IPositionalPlayer _partner;
    public ITeamBuilderPartner WithPlayer(IStartingPlayer player) => SetProperty(ref _player, player);
    public ITeamBuilderPartner WithPlayer(IOpposingPlayer player) => SetProperty(ref _player, player);
    public ITeamBuilderBuild WithPartner(IStartingPlayer partner) => SetProperty(ref _partner, partner);
    public ITeamBuilderBuild WithPartner(IOpposingPlayer partner) => SetProperty(ref _partner, partner);

    protected override ITeam BuildInternal()
    {
        if (_player is not { } player) return UnknownTeam.Instance;
        if (_partner is not { } partner) return UnknownTeam.Instance;
    }
}

