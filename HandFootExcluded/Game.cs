using Bertuzzi.MAUI.EventAggregator;

namespace HandFootExcluded;

public interface IGame : ISet<IRound>
{
    IList<IPlayer> Players { get; }
}

internal sealed partial class GameService
{
    internal sealed class Game : HashSet<IRound>, IGame
    {
        private readonly IScoringService _scoringService;
        public IList<IPlayer> Players { get; }

        public Game(IList<IPlayer> players, IScoringService scoringService)
        {
            EventAggregator.Instance.UnregisterHandler<string>(Score);

            _scoringService = scoringService;
            Players = players ?? throw new ArgumentNullException(nameof(players));

            EventAggregator.Instance.RegisterHandler<string>(Score);
        }

        private void Score(string obj)
        {
            var playerScores = Players.Select(player => _scoringService.CalculateFor(this, player)).ToList();

            EventAggregator.Instance.SendMessage(playerScores);
        }
    }
}