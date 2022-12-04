using System.Diagnostics;
using Bertuzzi.MAUI.EventAggregator;

namespace HandFootExcluded;

public interface IRound
{
    int Index { get; }
    int AmountToOpen { get; }
    IPlayer StartingPlayer { get; }
    IPlayer StartingPartner { get; }
    IPlayer OpposingPlayer { get; }
    IPlayer OpposingPartner { get; }
    IPlayer ExcludedPlayer { get; }
    ITeam StartingTeam { get; }
    ITeam OpposingTeam { get; }

    bool StartingPlayerBonus { get; }
    bool StartingPartnerBonus { get; }
    bool OpposingPlayerBonus { get; }
    bool OpposingPartnerBonus { get; }
}

internal sealed partial class GameService
{
    [DebuggerDisplay("{Display,nq}")]
    private sealed class Round : BindableItem, IRound
    {
        private bool _startingPlayerBonus;
        private bool _startingPartnerBonus;
        private bool _opposingPlayerBonus;
        private bool _opposingPartnerBonus;

        public int Index { get; }
        public int AmountToOpen { get; }
        public IPlayer StartingPlayer { get; }
        public IPlayer StartingPartner { get; }
        public IPlayer OpposingPlayer { get; }
        public IPlayer OpposingPartner { get; }
        public IPlayer ExcludedPlayer { get; }
        public ITeam StartingTeam { get; }
        public ITeam OpposingTeam { get; }

        public bool StartingPlayerBonus { get => _startingPlayerBonus; set => SetProperty(ref _startingPlayerBonus, value, OnBonusChanged); }
        public bool StartingPartnerBonus { get => _startingPartnerBonus; set => SetProperty(ref _startingPartnerBonus, value, OnBonusChanged); }
        public bool OpposingPlayerBonus { get => _opposingPlayerBonus; set => SetProperty(ref _opposingPlayerBonus, value, OnBonusChanged); }
        public bool OpposingPartnerBonus { get => _opposingPartnerBonus; set => SetProperty(ref _opposingPartnerBonus, value, OnBonusChanged); }

        private string Display => $"{StartingTeam} vs. {OpposingTeam} ||  ({ExcludedPlayer})";

        public Round(int index, int amountToOpen, IPlayer startingPlayer, IPlayer startingPartner, IPlayer opposingPlayer, IPlayer opposingPartner, IPlayer excludedPlayer)
        {
            Index = index;
            AmountToOpen = amountToOpen;
            StartingPlayer = startingPlayer;
            StartingPartner = startingPartner;
            OpposingPlayer = opposingPlayer;
            OpposingPartner = opposingPartner;
            ExcludedPlayer = excludedPlayer;
            StartingTeam = BuildTeam(startingPlayer, startingPartner);
            OpposingTeam = BuildTeam(opposingPlayer, opposingPartner);
        }

        private void OnBonusChanged() { EventAggregator.Instance.SendMessage("Score"); }

        private static ITeam BuildTeam(IPlayer player, IPlayer partner) => new Team(player, partner);

        public override string ToString() => Display;
    }

    public class ScoringHandler { }
}