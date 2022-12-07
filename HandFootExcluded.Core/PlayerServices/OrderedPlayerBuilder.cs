using HandFootExcluded.Common;

namespace HandFootExcluded.Core.PlayerServices;

public interface IOrderedPlayerBuilder : IBuilder
{
    IOrderedPlayerOrder WithPlayer(INonPositionalPlayer nonPositionalPlayer);
}

public interface IOrderedPlayerOrder
{
    IOrderedPlayerBuilderBuild WithOrder(int order);
}

public interface IOrderedPlayerBuilderBuild : IBuilder<IOrderedPlayer> { }

internal class OrderedPlayerBuilder : BuilderBase<OrderedPlayerBuilder, IOrderedPlayer>, IOrderedPlayerBuilder, IOrderedPlayerOrder, IOrderedPlayerBuilderBuild
{
    private INonPositionalPlayer _nonPositionalPlayer;
    private int _order;
    public IOrderedPlayerOrder WithPlayer(INonPositionalPlayer nonPositionalPlayer) => SetProperty(ref _nonPositionalPlayer, nonPositionalPlayer);
    public IOrderedPlayerBuilderBuild WithOrder(int order) => SetProperty(ref _order, order);

    protected override IOrderedPlayer BuildInternal()
    {
        if (_order is <= 0 or > 5) return UnknownPlayer.Instance;

        return new OrderedPlayer(_order, _nonPositionalPlayer.FullName, _nonPositionalPlayer.FirstName, _nonPositionalPlayer.MiddleName, _nonPositionalPlayer.LastName, _nonPositionalPlayer.Initials);
    }
}