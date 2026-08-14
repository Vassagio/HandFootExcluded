using HandFootExcluded.UI.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IScoreLine
{
    int Order {get;}
    string Name {get;}
    string Initials { get; }
    int Value { get; }
    bool IsBold {get;}
    int FontSize {get;}
}

internal abstract class ScoreLineBase : IScoreLine
{
    public abstract int Order {get;}
    public abstract string Name { get; }
    public string Initials { get; set; }
    public int Value { get; set; }
    protected abstract string Display {get;}
    public virtual bool IsBold => false;
    public virtual int FontSize => 15;

    protected ScoreLineBase()
    {

    }

    protected ScoreLineBase(string initials, int value)
    {
        Initials = initials;
        Value = value;
    }

    public override string ToString() => Display;
}