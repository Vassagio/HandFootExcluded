using System.Windows.Input;
using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.UI.Eventing;
using HandFootExcluded.UI.Services;
using HandFootExcluded.UI.Services.ConfigurationServices;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface ISettingsPageViewModel : IViewModel
{
    string Player1 { get; }
    string Player2 { get; }
    string Player3 { get; }
    string Player4 { get; }
    string Player5 { get; }

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
    private readonly IConfigurationManager _configurationManager;
    private int _bonusAmount;

    private int _maxDiscardPickup;
    private int _minDiscardPickup;

    private ICommand _okCommand;

    private string _player1;
    private string _player2;
    private string _player3;
    private string _player4;
    private string _player5;

    private int _roundOpening1;
    private int _roundOpening2;
    private int _roundOpening3;
    private int _roundOpening4;
    private int _roundOpening5;

    public SettingsPageViewModel(ISettings settings)
    {
        Player1 = settings.Player1;
        Player2 = settings.Player2;
        Player3 = settings.Player3;
        Player4 = settings.Player4;
        Player5 = settings.Player5;

        RoundOpening1 = settings.RoundOpening1;
        RoundOpening2 = settings.RoundOpening2;
        RoundOpening3 = settings.RoundOpening3;
        RoundOpening4 = settings.RoundOpening4;
        RoundOpening5 = settings.RoundOpening5;

        BonusAmount = settings.BonusAmount;
        BonusAmount = settings.BonusAmount;
        BonusAmount = settings.BonusAmount;
    }

    public string Player1 { get => _player1; set => SetProperty(ref _player1, value); }
    public string Player2 { get => _player2; set => SetProperty(ref _player2, value); }
    public string Player3 { get => _player3; set => SetProperty(ref _player3, value); }
    public string Player4 { get => _player4; set => SetProperty(ref _player4, value); }
    public string Player5 { get => _player5; set => SetProperty(ref _player5, value); }

    public int BonusAmount { get => _bonusAmount; set => SetProperty(ref _bonusAmount, value); }
    public int MinDiscardPickup { get => _minDiscardPickup; set => SetProperty(ref _minDiscardPickup, value); }
    public int MaxDiscardPickup { get => _maxDiscardPickup; set => SetProperty(ref _maxDiscardPickup, value); }

    public int RoundOpening1 { get => _roundOpening1; set => SetProperty(ref _roundOpening1, value); }
    public int RoundOpening2 { get => _roundOpening2; set => SetProperty(ref _roundOpening2, value); }
    public int RoundOpening3 { get => _roundOpening3; set => SetProperty(ref _roundOpening3, value); }
    public int RoundOpening4 { get => _roundOpening4; set => SetProperty(ref _roundOpening4, value); }
    public int RoundOpening5 { get => _roundOpening5; set => SetProperty(ref _roundOpening5, value); }

    public ICommand OkCommand => SetCommand(ref _okCommand, Ok);

    private void Ok()
    {
        var players = new List<string>
        {
            _player1,
            _player2,
            _player3,
            _player4,
            _player5
        };

        var roundOpenings = new List<int>
        {
            _roundOpening1,
            _roundOpening2,
            _roundOpening3,
            _roundOpening4,
            _roundOpening5
        };

        _configurationManager.Save(this);
        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new SettingsChangedEvent(players, roundOpenings, _bonusAmount, _minDiscardPickup, _maxDiscardPickup));
    }
}