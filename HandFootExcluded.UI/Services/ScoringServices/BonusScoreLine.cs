using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace HandFootExcluded.UI.Services.ScoringServices;

public interface IBonusScoreLine : IRoundScoreLine { }

internal sealed class BonusScoreLine : RoundScoreLineBase, IBonusScoreLine
{
    public override int Order => 1;
    public static readonly int BONUS_AMOUNT = 100;
    public override string Name => "Bonus";
    
    public BonusScoreLine(int roundOrder, string initials, bool hasBonus) : base(roundOrder, initials, hasBonus ? BONUS_AMOUNT : 0) { }
}