using HandFootExcluded.Services;

namespace HandFootExcluded.Events;

public sealed class GameFinishedEvent
{
    public IGame Game{ get; }

    public GameFinishedEvent(IGame game) => Game = game;
}