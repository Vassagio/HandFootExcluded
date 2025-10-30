using HandFootExcluded.Core.PlayerServices;

namespace HandFootExcluded.UI.Eventing;

internal sealed class SettingsChangedEvent
{
    public IEnumerable<IPlayerSetting> Players { get; }
    public IEnumerable<int> RoundOpeningAmounts { get; }
    public int BonusAmount { get; }
    public int MinDiscardPickup { get; }
    public int MaxDiscardPickup { get; }

    public SettingsChangedEvent(IEnumerable<IPlayerSetting> players, IEnumerable<int> roundOpeningAmounts, int bonusAmount, int minDiscardPickup, int maxDiscardPickup)
    {
        Players = players;
        RoundOpeningAmounts = roundOpeningAmounts;
        BonusAmount = bonusAmount;
        MinDiscardPickup = minDiscardPickup;
        MaxDiscardPickup = maxDiscardPickup;
    }
}