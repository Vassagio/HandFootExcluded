namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLine
{
    int Order {get;}
    string Name {get;}
    string Initials { get; }
    int Value { get; }
}

internal abstract class ScoreLineBase : IScoreLine
{
    public abstract int Order {get;}
    public abstract string Name { get; }
    public string Initials { get; }
    public int Value { get; }
    protected abstract string Display {get;}

    protected ScoreLineBase(string initials, int value)
    {
        Initials = initials;
        Value = value;
    }

    public override string ToString() => Display;
}