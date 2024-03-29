﻿using Bertuzzi.MAUI.EventAggregator;
using HandFootExcluded.Common;
using HandFootExcluded.Services;

namespace HandFootExcluded;

public interface ISettingsViewModel
{
    string Title {get; }
    string DefaultPlayer1 { get; }
    string DefaultPlayer2 { get; }
    string DefaultPlayer3 { get; }
    string DefaultPlayer4 { get; }
    string DefaultPlayer5 { get; }

    Command StartCommand { get; }
}

internal sealed class SettingsViewModel : BindableItem, ISettingsViewModel
{
    private readonly IPlayerBuilder _playerBuilder;

    private string _title = AppInfo.Current.Name;
    private string _defaultPlayer1 = "William Christopher Chronowski";
    private string _defaultPlayer2 = "Tami Renae Chronowski";
    private string _defaultPlayer3 = "Kaelia Shyenne Chronowski";
    private string _defaultPlayer4 = "Korian Alexa Chronowski";
    private string _defaultPlayer5 = "Jay Michael Looney";
    private IList<string> _defaultPlayers = new List<string>();

    private Command _startCommand;

    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string DefaultPlayer1 { get => _defaultPlayer1; set => SetProperty(ref _defaultPlayer1, value.Trim(), OnPlayerChanged); }
    public string DefaultPlayer2 { get => _defaultPlayer2; set => SetProperty(ref _defaultPlayer2, value.Trim(), OnPlayerChanged); }
    public string DefaultPlayer3 { get => _defaultPlayer3; set => SetProperty(ref _defaultPlayer3, value.Trim(), OnPlayerChanged); }
    public string DefaultPlayer4 { get => _defaultPlayer4; set => SetProperty(ref _defaultPlayer4, value.Trim(), OnPlayerChanged); }
    public string DefaultPlayer5 { get => _defaultPlayer5; set => SetProperty(ref _defaultPlayer5, value.Trim(), OnPlayerChanged); }

    public Command StartCommand => _startCommand ?? new Command(Start, CanStart);

    public SettingsViewModel(IPlayerBuilder playerBuilder)
    {
        _playerBuilder = playerBuilder ?? throw new ArgumentNullException(nameof(playerBuilder));

        OnPlayerChanged();
    }

    private void OnPlayerChanged()
    {
        _defaultPlayers = new List<string>
        {
            _defaultPlayer1,
            _defaultPlayer2,
            _defaultPlayer3,
            _defaultPlayer4,
            _defaultPlayer5
        };
    }

    private bool CanStart() =>
        !string.IsNullOrWhiteSpace(_defaultPlayer1) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer2) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer3) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer4) &&
        !string.IsNullOrWhiteSpace(_defaultPlayer5);

    private void Start()
    {
        _defaultPlayers.Shuffle();

        var players = new Players();
        for (var i = 1; i <= _defaultPlayers.Count; i++)
        {
            var player = _playerBuilder.WithPosition(i).WithName(_defaultPlayers[i - 1]).Build();
            players.Add(player);
        }

        EventAggregator.Instance.SendMessage(players);
    }
}