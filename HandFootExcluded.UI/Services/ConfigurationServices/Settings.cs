namespace HandFootExcluded.UI.Services.ConfigurationServices;

public interface ISettings
{
    string Player1 { get; }
    string Player2 { get; }
    string Player3 { get; }
    string Player4 { get; }
    string Player5 { get; }

    int RoundOpening1 { get; }
    int RoundOpening2 { get; }
    int RoundOpening3 { get; }
    int RoundOpening4 { get; }
    int RoundOpening5 { get; }

    int BonusAmount { get; }
    int MinDiscardPickup { get; }
    int MaxDiscardPickup { get; }
}

internal sealed partial class ConfigurationManager
{
    private sealed class Settings : ISettings
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public string Player5 { get; set; }
        public int RoundOpening1 { get; set; }
        public int RoundOpening2 { get; set; }
        public int RoundOpening3 { get; set; }
        public int RoundOpening4 { get; set; }
        public int RoundOpening5 { get; set; }
        public int BonusAmount { get; set; }
        public int MinDiscardPickup { get; set; }
        public int MaxDiscardPickup { get; set; }
    }
}