using HandFootExcluded.Services;

namespace HandFootExcluded;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new AppShell(new DeviceOrientationService()));
    }
}