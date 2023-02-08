namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLine
{
    int Order { get; }
    string Name { get; }
    string Initials { get; }
    int Value { get; }
    bool IsBold { get; }
    int FontSize { get; }
}

internal abstract class ScoreLineBase : IScoreLine
{
    protected abstract string Display { get; }

    protected ScoreLineBase(string initials, int value)
    {
        Initials = initials;
        Value = value;
    }
    public abstract int Order { get; }
    public abstract string Name { get; }
    public string Initials { get; }
    public int Value { get; }
    public virtual bool IsBold => false;
    public virtual int FontSize => 15;

    public override string ToString() => Display;
}