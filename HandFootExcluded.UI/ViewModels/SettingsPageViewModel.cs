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

    ICommand OkCommand { get; }
}

internal sealed class SettingsPageViewModel : ViewModelBase, ISettingsPageViewModel
{
    private string _defaultPlayer1 = "William Christopher Chronowski";
    private string _defaultPlayer2 = "Tami Renae Chronowski";
    private string _defaultPlayer3 = "Kaelia Shyenne Chronowski";
    private string _defaultPlayer4 = "Korian Alexa Chronowski";
    private string _defaultPlayer5 = "Jay Michael Looney";

    private Command _okCommand;

    public string DefaultPlayer1 { get => _defaultPlayer1; set => SetProperty(ref _defaultPlayer1, value); }
    public string DefaultPlayer2 { get => _defaultPlayer2; set => SetProperty(ref _defaultPlayer2, value); }
    public string DefaultPlayer3 { get => _defaultPlayer3; set => SetProperty(ref _defaultPlayer3, value); }
    public string DefaultPlayer4 { get => _defaultPlayer4; set => SetProperty(ref _defaultPlayer4, value); }
    public string DefaultPlayer5 { get => _defaultPlayer5; set => SetProperty(ref _defaultPlayer5, value); }

    public ICommand OkCommand => _okCommand ?? new Command(Ok);



    private void Ok()
    {
        var players = new List<string>
        {
            _defaultPlayer1,
            _defaultPlayer2,
            _defaultPlayer3,
            _defaultPlayer4,
            _defaultPlayer5
        };

        Navigate<IGamePage>();

        EventAggregator.Instance.SendMessage(new PlayersChangedEvent(players));
    }
}