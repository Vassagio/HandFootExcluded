using System.Windows.Input;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface ISettingsPageViewModel : IViewModel
{
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

    int BonusAmount { get; }
    int MinDiscardPickup { get; }
    int MaxDiscardPickup { get; }

    int RoundOpening1 { get; }
    int RoundOpening2 { get; }
    int RoundOpening3 { get; }
    int RoundOpening4 { get; }
    int RoundOpening5 { get; }

    ICommand OkCommand { get; }
}

internal sealed class SettingsPageViewModel : ViewModelBase, ISettingsPageViewModel
{
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

    private int _bonusAmount = 250;
    private int _minDiscardPickup = 4;
    private int _maxDiscardPickup = 10;

    private int _roundOpening1 = 100;
    private int _roundOpening2 = 100;
    private int _roundOpening3 = 100;
    private int _roundOpening4 = 100;
    private int _roundOpening5 = 100;

    private Command _okCommand;

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

    public int BonusAmount { get => _bonusAmount; set => SetProperty(ref _bonusAmount, value); }
    public int MinDiscardPickup { get => _minDiscardPickup; set => SetProperty(ref _minDiscardPickup, value); }
    public int MaxDiscardPickup { get => _maxDiscardPickup; set => SetProperty(ref _maxDiscardPickup, value); }

    public int RoundOpening1 { get => _roundOpening1; set => SetProperty(ref _roundOpening1, value); }
    public int RoundOpening2 { get => _roundOpening2; set => SetProperty(ref _roundOpening2, value); }
    public int RoundOpening3 { get => _roundOpening3; set => SetProperty(ref _roundOpening3, value); }
    public int RoundOpening4 { get => _roundOpening4; set => SetProperty(ref _roundOpening4, value); }
    public int RoundOpening5 { get => _roundOpening5; set => SetProperty(ref _roundOpening5, value); }

    public ICommand OkCommand => _okCommand ?? new Command(Ok);

    private void Ok()
    {
        var players = new List<IPlayerSetting>
        {
            PlayerSetting.Create(_defaultPlayer1, _excludedPlayer1),
            PlayerSetting.Create(_defaultPlayer2, _excludedPlayer2),
            PlayerSetting.Create(_defaultPlayer3, _excludedPlayer3),
            PlayerSetting.Create(_defaultPlayer4, _excludedPlayer4),
            PlayerSetting.Create(_defaultPlayer5, _excludedPlayer5)
        };

        var roundOpenings = new List<int>
        {
            _roundOpening1,
            _roundOpening2,
            _roundOpening3,
            _roundOpening4,
            _roundOpening5
        };

        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new SettingsChangedEvent(players, roundOpenings, _bonusAmount, _minDiscardPickup, _maxDiscardPickup));
    }
}