namespace HandFootExcluded.UI.Eventing;

internal sealed class PlayersChangedEvent
{
    public IEnumerable<string> Players { get; }

    public PlayersChangedEvent(IEnumerable<string> players) => Players = players;
}