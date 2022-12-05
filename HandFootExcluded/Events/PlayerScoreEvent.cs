namespace HandFootExcluded.Events;

public sealed class PlayerScoreEvent
{
    public static readonly PlayerScoreEvent Yes = new(true);
    public static readonly PlayerScoreEvent No = new(false);

    public bool Score {get;}

    private PlayerScoreEvent(bool score) => Score = score;
}