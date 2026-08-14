using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.UI.Eventing;

internal sealed class SettingsChangedEvent
{
    public IEnumerable<IPlayerSetting> Players { get; }
    public IEnumerable<int> RoundOpeningAmounts { get; }
    public IDictionary<int, int> IndividualBonusAmounts { get; }
    public int TeamBonusAmount { get; }
    public int MinDiscardPickup { get; }
    public int MaxDiscardPickup { get; }

    public SettingsChangedEvent(IEnumerable<IPlayerSetting> players, IEnumerable<int> roundOpeningAmounts, IDictionary<int, int> individualBonusAmounts, int teamBonusAmount, int minDiscardPickup, int maxDiscardPickup)
    {
        Players = players;
        RoundOpeningAmounts = roundOpeningAmounts;
        IndividualBonusAmounts = individualBonusAmounts;
        TeamBonusAmount = teamBonusAmount;
        MinDiscardPickup = minDiscardPickup;
        MaxDiscardPickup = maxDiscardPickup;
    }
}