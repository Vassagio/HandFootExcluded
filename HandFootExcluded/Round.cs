using HandFootExcluded;

namespace HandFootExcluded;

public interface IRound
{
    int Index {get;}
    IPlayer StartingPlayer { get; }
    IPlayer StartingPartner { get; }
    IPlayer OpposingPlayer { get; }
    IPlayer OpposingPartner { get; }
    IPlayer ExcludedPlayer { get; }
    ITeam StartingTeam { get; }
    ITeam OpposingTeam { get; }

    bool StartingPlayerBonus {get;}
    bool StartingPartnerBonus { get; }
    bool OpposingPlayerBonus { get; }
    bool OpposingPartnerBonus { get; }
}

internal sealed partial class GameService
{
    internal sealed class Round : BindableItem, IRound
    {
        private bool _startingPlayerBonus;
        private bool _startingPartnerBonus;
        private bool _opposingPlayerBonus;
        private bool _opposingPartnerBonus;

        public int Index { get; }
        public IPlayer StartingPlayer { get; }
        public IPlayer StartingPartner { get; }
        public IPlayer OpposingPlayer { get; }
        public IPlayer OpposingPartner { get; }
        public IPlayer ExcludedPlayer { get; }
        public ITeam StartingTeam { get; }
        public ITeam OpposingTeam { get; }

        public bool StartingPlayerBonus { get => _startingPlayerBonus; set => SetProperty(ref _startingPlayerBonus, value); }
        public bool StartingPartnerBonus { get => _startingPartnerBonus; set => SetProperty(ref _startingPartnerBonus, value); }
        public bool OpposingPlayerBonus { get => _opposingPlayerBonus; set => SetProperty(ref _opposingPlayerBonus, value); }
        public bool OpposingPartnerBonus { get => _opposingPartnerBonus; set => SetProperty(ref _opposingPartnerBonus, value); }

        public Round(int index, IPlayer startingPlayer, IPlayer startingPartner, IPlayer opposingPlayer, IPlayer opposingPartner, IPlayer excludedPlayer)
        {
            Index = index;
            StartingPlayer = startingPlayer;
            StartingPartner = startingPartner;
            OpposingPlayer = opposingPlayer;
            OpposingPartner = opposingPartner;
            ExcludedPlayer = excludedPlayer;
            StartingTeam = BuildTeam(startingPlayer, startingPartner);
            OpposingTeam = BuildTeam(opposingPlayer, opposingPartner);
        }

        private static ITeam BuildTeam(IPlayer player, IPlayer partner) => new Team(player, partner);

        public override string ToString() => $"{StartingTeam} vs. {OpposingTeam} ||  ({ExcludedPlayer})";
    }
}