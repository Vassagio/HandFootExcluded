using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Eventing;

internal sealed class GameLoadedEvent
{
    public IGamePageViewModel GamePageViewModel { get; }
    public ISettingsPageViewModel SettingsPageViewModel { get; }

    public GameLoadedEvent(IGamePageViewModel gamePageViewModel, ISettingsPageViewModel settingsPageViewModel)
    {
        GamePageViewModel = gamePageViewModel;
        SettingsPageViewModel = settingsPageViewModel;
    }
}