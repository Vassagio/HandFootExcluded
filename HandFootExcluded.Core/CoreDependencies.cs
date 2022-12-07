using HandFootExcluded.Common;
using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.Core.RoundServices;
using HandFootExcluded.Core.TeamServices;

namespace HandFootExcluded.Core;

public class CoreDependencies : IDependencyExtension
{
    public IServiceCollection Initialize(IServiceCollection services) =>
        services.AddSingleton<IGameService, GameService>()
                .AddSingleton<IGameBuilder, GameBuilder>()
                .AddSingleton<INonPositionalPlayerBuilder, NonPositionalPlayerBuilder>()
                .AddSingleton<IOrderedPlayerBuilder, OrderedPlayerBuilder>()
                .AddSingleton<ITeamBuilder, TeamBuilder>()
                .AddSingleton<IRoundBuilder, RoundBuilder>()
                .AddSingleton<IPositionalPlayerFactory, PositionalPlayerFactory>();
}