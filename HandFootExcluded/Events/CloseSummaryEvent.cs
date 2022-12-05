namespace HandFootExcluded.Events;

public sealed class CloseSummaryEvent
{
    public static readonly CloseSummaryEvent Instance = new();

    private CloseSummaryEvent() { }
}