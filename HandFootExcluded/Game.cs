
namespace HandFootExcluded;

public interface IGame : ISet<IRound>
{
    IList<IPlayer> Players { get;}
}

internal sealed partial class GameService
{
    internal sealed class Game : HashSet<IRound>, IGame
    {
        public IList<IPlayer> Players { get; }

        public Game(IList<IPlayer> players) => Players = players ?? throw new ArgumentNullException(nameof(players));
    }
}