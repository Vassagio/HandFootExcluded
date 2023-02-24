using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Services.ConfigurationServices;



public interface IConfigurationManager
{
    void Save(ISettingsPageViewModel settingsPageViewModel);
    Task<ISettings> Load();
}

internal sealed partial class ConfigurationManager : IConfigurationManager
{
    private const int DefaultBonusAmount = 250;
    private const int DefaultMaxDiscardPickup = 10;
    private const int DefaultMinDiscardPickup = 4;

    private const string DefaultPlayer1 = "William Christopher Chronowski";
    private const string DefaultPlayer2 = "Tami Renae Chronowski";
    private const string DefaultPlayer3 = "Kaelia Shyenne Chronowski";
    private const string DefaultPlayer4 = "Korian Alexa Chronowski";
    private const string DefaultPlayer5 = "Jay Michael Looney";

    private const int DefaultRoundOpening1 = 50;
    private const int DefaultRoundOpening2 = 75;
    private const int DefaultRoundOpening3 = 100;
    private const int DefaultRoundOpening4 = 125;
    private const int DefaultRoundOpening5 = 150;
    private readonly ISecureStorage _secureStorage;

    public ConfigurationManager(ISecureStorage secureStorage) => _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(secureStorage));

    public async void Save(ISettingsPageViewModel settingsPageViewModel)
    {
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.Player1), settingsPageViewModel.Player1);
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.Player2), settingsPageViewModel.Player2);
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.Player3), settingsPageViewModel.Player3);
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.Player4), settingsPageViewModel.Player4);
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.Player5), settingsPageViewModel.Player5);

        await _secureStorage.SetAsync(nameof(settingsPageViewModel.RoundOpening1), settingsPageViewModel.RoundOpening1.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.RoundOpening2), settingsPageViewModel.RoundOpening2.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.RoundOpening3), settingsPageViewModel.RoundOpening3.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.RoundOpening4), settingsPageViewModel.RoundOpening4.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.RoundOpening5), settingsPageViewModel.RoundOpening5.ToString());

        await _secureStorage.SetAsync(nameof(settingsPageViewModel.BonusAmount), settingsPageViewModel.BonusAmount.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.MinDiscardPickup), settingsPageViewModel.MinDiscardPickup.ToString());
        await _secureStorage.SetAsync(nameof(settingsPageViewModel.MaxDiscardPickup), settingsPageViewModel.MaxDiscardPickup.ToString());
    }

    public async Task<ISettings> Load()
    {
        var settings = new Settings();
        settings.Player1 = await Load(nameof(settings.Player1), DefaultPlayer1);
        settings.Player2 = await Load(nameof(settings.Player2), DefaultPlayer2);
        settings.Player3 = await Load(nameof(settings.Player3), DefaultPlayer3);
        settings.Player4 = await Load(nameof(settings.Player4), DefaultPlayer4);
        settings.Player5 = await Load(nameof(settings.Player5), DefaultPlayer5);

        settings.RoundOpening1 = await Load(nameof(settings.RoundOpening1), DefaultRoundOpening1);
        settings.RoundOpening2 = await Load(nameof(settings.RoundOpening2), DefaultRoundOpening2);
        settings.RoundOpening3 = await Load(nameof(settings.RoundOpening3), DefaultRoundOpening3);
        settings.RoundOpening4 = await Load(nameof(settings.RoundOpening4), DefaultRoundOpening4);
        settings.RoundOpening5 = await Load(nameof(settings.RoundOpening5), DefaultRoundOpening5);

        settings.BonusAmount = await Load(nameof(settings.BonusAmount), DefaultBonusAmount);
        settings.MinDiscardPickup = await Load(nameof(settings.MinDiscardPickup), DefaultMinDiscardPickup);
        settings.MaxDiscardPickup = await Load(nameof(settings.MaxDiscardPickup), DefaultMaxDiscardPickup);

        return settings;
    }

    private async Task<T> Load<T>(string key, T @default)
    {
        if (string.IsNullOrWhiteSpace(key))
            return @default;
        var value = await _secureStorage.GetAsync(key);
        if (string.IsNullOrWhiteSpace(value))
            return @default;

        var converted = Convert.ChangeType(value, typeof(T));

        return converted is T result ? result : @default;
    }
}