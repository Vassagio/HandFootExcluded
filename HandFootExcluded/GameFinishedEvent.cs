namespace HandFootExcluded;

public sealed class GameFinishedEvent
{
    public IGame Game{ get; }

    public GameFinishedEvent(IGame game) => Game = game;
}