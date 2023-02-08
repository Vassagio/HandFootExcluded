using System.Windows.Input;
using Bertuzzi.MAUI.EventAggregator;
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
    private readonly ISecureStorage _secureStorage;

    private int _bonusAmount;

    private readonly int _defaultBonusAmount = 250;
    private readonly int _defaultMaxDiscardPickup = 10;
    private readonly int _defaultMinDiscardPickup = 4;

    private readonly string _defaultPlayer1 = "William Christopher Chronowski";
    private readonly string _defaultPlayer2 = "Tami Renae Chronowski";
    private readonly string _defaultPlayer3 = "Kaelia Shyenne Chronowski";
    private readonly string _defaultPlayer4 = "Korian Alexa Chronowski";
    private readonly string _defaultPlayer5 = "Jay Michael Looney";

    private readonly int _defaultRoundOpening1 = 50;
    private readonly int _defaultRoundOpening2 = 75;
    private readonly int _defaultRoundOpening3 = 100;
    private readonly int _defaultRoundOpening4 = 125;
    private readonly int _defaultRoundOpening5 = 150;
    private int _maxDiscardPickup;
    private int _minDiscardPickup;

    private Command _okCommand;

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

    public SettingsPageViewModel(ISecureStorage secureStorage)
    {
        _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(secureStorage));
        Load();
    }

    public string DefaultPlayer1 { get => _player1; set => SetProperty(ref _player1, value); }
    public string DefaultPlayer2 { get => _player2; set => SetProperty(ref _player2, value); }
    public string DefaultPlayer3 { get => _player3; set => SetProperty(ref _player3, value); }
    public string DefaultPlayer4 { get => _player4; set => SetProperty(ref _player4, value); }
    public string DefaultPlayer5 { get => _player5; set => SetProperty(ref _player5, value); }

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

        Save();
        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new SettingsChangedEvent(players, roundOpenings, _bonusAmount, _minDiscardPickup, _maxDiscardPickup));
    }

    private async void Save()
    {
        await _secureStorage.SetAsync(nameof(DefaultPlayer1), DefaultPlayer1);
        await _secureStorage.SetAsync(nameof(DefaultPlayer2), DefaultPlayer2);
        await _secureStorage.SetAsync(nameof(DefaultPlayer3), DefaultPlayer3);
        await _secureStorage.SetAsync(nameof(DefaultPlayer4), DefaultPlayer4);
        await _secureStorage.SetAsync(nameof(DefaultPlayer5), DefaultPlayer5);

        await _secureStorage.SetAsync(nameof(RoundOpening1), RoundOpening1.ToString());
        await _secureStorage.SetAsync(nameof(RoundOpening2), RoundOpening2.ToString());
        await _secureStorage.SetAsync(nameof(RoundOpening3), RoundOpening3.ToString());
        await _secureStorage.SetAsync(nameof(RoundOpening4), RoundOpening4.ToString());
        await _secureStorage.SetAsync(nameof(RoundOpening5), RoundOpening5.ToString());

        await _secureStorage.SetAsync(nameof(BonusAmount), BonusAmount.ToString());
        await _secureStorage.SetAsync(nameof(MinDiscardPickup), MinDiscardPickup.ToString());
        await _secureStorage.SetAsync(nameof(MaxDiscardPickup), MaxDiscardPickup.ToString());
    }

    private async void Load()
    {
        DefaultPlayer1 = await Load(nameof(DefaultPlayer1), _defaultPlayer1);
        DefaultPlayer2 = await Load(nameof(DefaultPlayer2), _defaultPlayer2);
        DefaultPlayer3 = await Load(nameof(DefaultPlayer3), _defaultPlayer3);
        DefaultPlayer4 = await Load(nameof(DefaultPlayer4), _defaultPlayer4);
        DefaultPlayer5 = await Load(nameof(DefaultPlayer5), _defaultPlayer5);

        RoundOpening1 = await Load(nameof(RoundOpening1), _defaultRoundOpening1);
        RoundOpening2 = await Load(nameof(RoundOpening2), _defaultRoundOpening2);
        RoundOpening3 = await Load(nameof(RoundOpening3), _defaultRoundOpening3);
        RoundOpening4 = await Load(nameof(RoundOpening4), _defaultRoundOpening4);
        RoundOpening5 = await Load(nameof(RoundOpening5), _defaultRoundOpening5);

        BonusAmount = await Load(nameof(BonusAmount), _defaultBonusAmount);
        MinDiscardPickup = await Load(nameof(MinDiscardPickup), _defaultMinDiscardPickup);
        MaxDiscardPickup = await Load(nameof(MaxDiscardPickup), _defaultMaxDiscardPickup);
    }

    private async Task<T> Load<T>(string key, T @default)
    {
        if (string.IsNullOrWhiteSpace(key)) return @default;
        var value = await _secureStorage.GetAsync(key);
        if (string.IsNullOrWhiteSpace(value)) return @default;

        var converted = Convert.ChangeType(value, typeof(T));

        return converted is T result ? result : @default;
    }
}