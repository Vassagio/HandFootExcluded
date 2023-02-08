using HandFootExcluded.UI.Services;

namespace HandFootExcluded.UI;

public partial class App : Application
{
    public App(IDeviceOrientationService deviceOrientationService)
    {
        InitializeComponent();

        MainPage = new AppShell(deviceOrientationService);
    }
}
