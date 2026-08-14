using AndroidX.ConstraintLayout.Helper.Widget;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Views;
using System.Windows.Input;
using HandFootExcluded.UI.Services;
using Xamarin.Google.Crypto.Tink.Proto;

namespace HandFootExcluded.UI.ViewModels;

public interface ISettingsPageViewModel : IViewModel
{
    List<IPlayerSetting> PlayerSettings { get; }

    string DefaultPlayer1 { get; }
    string DefaultPlayer2 { get; }
    string DefaultPlayer3 { get; }
    string DefaultPlayer4 { get; }
    string DefaultPlayer5 { get; }

    bool RandomExcludedPlayer { get; }
    bool ExcludedPlayer1 { get; }
    bool ExcludedPlayer2 { get; }
    bool ExcludedPlayer3 { get; }
    bool ExcludedPlayer4 { get; }
    bool ExcludedPlayer5 { get; }

    IDictionary<int, int> IndividualBonusAmounts { get; }
    int NoIndividualBonusAmount { get; }
    int IndividualBonusAmount1 { get; }
    int IndividualBonusAmount2 { get; }
    int IndividualBonusAmount3 { get; }
    int IndividualBonusAmount4 { get; }
    int TeamBonusAmount { get; }
    int MinDiscardPickup { get; }
    int MaxDiscardPickup { get; }

    List<int> RoundOpenings { get;  }
    int RoundOpening1 { get; }
    int RoundOpening2 { get; }
    int RoundOpening3 { get; }
    int RoundOpening4 { get; }
    int RoundOpening5 { get; }

    ICommand OkCommand { get; }
    ICommand LoadCommand { get; }
}

internal sealed class SettingsPageViewModel : ViewModelBase, ISettingsPageViewModel
{
    private readonly IJsonService _jsonService;
    private readonly IAlertService _alertService;
    private string _defaultPlayer1 = "William Christopher Chronowski";
    private string _defaultPlayer2 = "Tami Renae Chronowski";
    private string _defaultPlayer3 = "Kaelia Shyenne Chronowski";
    private string _defaultPlayer4 = "Korian Alexa Chronowski";
    private string _defaultPlayer5 = "Jay Michael Looney";

    private bool _randomExcludedPlayer = true;
    private bool _excludedPlayer1 = false;
    private bool _excludedPlayer2 = false;
    private bool _excludedPlayer3 = false;
    private bool _excludedPlayer4 = false;
    private bool _excludedPlayer5 = false;

    private int _noIndividualBonusAmount = 0;
    private int _individualBonusAmount1 = 250;
    private int _individualBonusAmount2 = 350;
    private int _individualBonusAmount3 = 500;
    private int _individualBonusAmount4 = 750;
    private int _teamBonusAmount = 100;
    private int _minDiscardPickup = 4;
    private int _maxDiscardPickup = 10;

    private int _roundOpening1 = 100;
    private int _roundOpening2 = 100;
    private int _roundOpening3 = 100;
    private int _roundOpening4 = 100;
    private int _roundOpening5 = 100;

    private Command _okCommand;
    private Command _loadCommand;

    public List<IPlayerSetting> PlayerSettings { get; set; }

    public string DefaultPlayer1 { get => _defaultPlayer1; set => SetProperty(ref _defaultPlayer1, value); }
    public string DefaultPlayer2 { get => _defaultPlayer2; set => SetProperty(ref _defaultPlayer2, value); }
    public string DefaultPlayer3 { get => _defaultPlayer3; set => SetProperty(ref _defaultPlayer3, value); }
    public string DefaultPlayer4 { get => _defaultPlayer4; set => SetProperty(ref _defaultPlayer4, value); }
    public string DefaultPlayer5 { get => _defaultPlayer5; set => SetProperty(ref _defaultPlayer5, value); }

    public bool RandomExcludedPlayer { get => _randomExcludedPlayer; set => SetProperty(ref _randomExcludedPlayer, value); }
    public bool ExcludedPlayer1 { get => _excludedPlayer1; set => SetProperty(ref _excludedPlayer1, value); }
    public bool ExcludedPlayer2 { get => _excludedPlayer2; set => SetProperty(ref _excludedPlayer2, value); }
    public bool ExcludedPlayer3 { get => _excludedPlayer3; set => SetProperty(ref _excludedPlayer3, value); }
    public bool ExcludedPlayer4 { get => _excludedPlayer4; set => SetProperty(ref _excludedPlayer4, value); }
    public bool ExcludedPlayer5 { get => _excludedPlayer5; set => SetProperty(ref _excludedPlayer5, value); }

    public IDictionary<int, int> IndividualBonusAmounts { get; set; }
    public int NoIndividualBonusAmount { get => _noIndividualBonusAmount; set => SetProperty(ref _noIndividualBonusAmount, value); }
    public int IndividualBonusAmount1 { get => _individualBonusAmount1; set => SetProperty(ref _individualBonusAmount1, value); }
    public int IndividualBonusAmount2 { get => _individualBonusAmount2; set => SetProperty(ref _individualBonusAmount2, value); }
    public int IndividualBonusAmount3 { get => _individualBonusAmount3; set => SetProperty(ref _individualBonusAmount3, value); }
    public int IndividualBonusAmount4 { get => _individualBonusAmount4; set => SetProperty(ref _individualBonusAmount4, value); }
    public int TeamBonusAmount { get => _teamBonusAmount; set => SetProperty(ref _teamBonusAmount, value); }
    public int MinDiscardPickup { get => _minDiscardPickup; set => SetProperty(ref _minDiscardPickup, value); }
    public int MaxDiscardPickup { get => _maxDiscardPickup; set => SetProperty(ref _maxDiscardPickup, value); }

    public List<int> RoundOpenings { get; set; }
    public int RoundOpening1 { get => _roundOpening1; set => SetProperty(ref _roundOpening1, value); }
    public int RoundOpening2 { get => _roundOpening2; set => SetProperty(ref _roundOpening2, value); }
    public int RoundOpening3 { get => _roundOpening3; set => SetProperty(ref _roundOpening3, value); }
    public int RoundOpening4 { get => _roundOpening4; set => SetProperty(ref _roundOpening4, value); }
    public int RoundOpening5 { get => _roundOpening5; set => SetProperty(ref _roundOpening5, value); }

    public ICommand OkCommand => _okCommand ?? new Command(Ok);
    public ICommand LoadCommand => _loadCommand ?? new Command(Load);


    public SettingsPageViewModel() {}
    public SettingsPageViewModel(IJsonService jsonService, IAlertService alertService)
    {
        _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));
        _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
    }

    private async void Ok()
    {
        PlayerSettings = new List<IPlayerSetting>
        {
            PlayerSetting.Create(_defaultPlayer1, _excludedPlayer1),
            PlayerSetting.Create(_defaultPlayer2, _excludedPlayer2),
            PlayerSetting.Create(_defaultPlayer3, _excludedPlayer3),
            PlayerSetting.Create(_defaultPlayer4, _excludedPlayer4),
            PlayerSetting.Create(_defaultPlayer5, _excludedPlayer5)
        };

        RoundOpenings = new List<int>
        {
            _roundOpening1,
            _roundOpening2,
            _roundOpening3,
            _roundOpening4,
            _roundOpening5
        };

        IndividualBonusAmounts = new Dictionary<int, int>
        {
            { 0, _noIndividualBonusAmount },
            { 1, _individualBonusAmount1 },
            { 2, _individualBonusAmount2 },
            { 3, _individualBonusAmount3 },
            { 4, _individualBonusAmount4 }
        };

        await _jsonService.SaveAsync<SettingsPageViewModel>(this, "settings.json");

        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new SettingsChangedEvent(PlayerSettings, RoundOpenings, IndividualBonusAmounts, TeamBonusAmount ,_minDiscardPickup, _maxDiscardPickup));
    }


    private async void Load()
    {
        var gamePageViewModel = await _jsonService.LoadAsync<GamePageViewModel>("game.json");
        var settingsPageViewModel = await _jsonService.LoadAsync<SettingsPageViewModel>("settings.json");

        if (gamePageViewModel is null || settingsPageViewModel is null)
        {
            await _alertService.ShowAlertAsync("Load Error", "No saved game found to load.  Starting new game.");
            Ok();
            return;
        }

        settingsPageViewModel.PlayerSettings = new List<IPlayerSetting>
        {
            PlayerSetting.Create(settingsPageViewModel.DefaultPlayer1, settingsPageViewModel.ExcludedPlayer1),
            PlayerSetting.Create(settingsPageViewModel.DefaultPlayer2, settingsPageViewModel.ExcludedPlayer2),
            PlayerSetting.Create(settingsPageViewModel.DefaultPlayer3, settingsPageViewModel.ExcludedPlayer3),
            PlayerSetting.Create(settingsPageViewModel.DefaultPlayer4, settingsPageViewModel.ExcludedPlayer4),
            PlayerSetting.Create(settingsPageViewModel.DefaultPlayer5, settingsPageViewModel.ExcludedPlayer5)
        };

        settingsPageViewModel.RoundOpenings = new List<int>
        {
            settingsPageViewModel.RoundOpening1,
            settingsPageViewModel.RoundOpening2,
            settingsPageViewModel.RoundOpening3,
            settingsPageViewModel.RoundOpening4,
            settingsPageViewModel.RoundOpening5
        };

        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new GameLoadedEvent(gamePageViewModel, settingsPageViewModel));
    }
}