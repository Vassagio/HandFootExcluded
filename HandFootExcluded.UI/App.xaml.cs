using HandFootExcluded.UI.Services;

namespace HandFootExcluded.UI;

public partial class App : Application
{
    private readonly IDeviceOrientationService _deviceOrientationService;

    public App(IDeviceOrientationService deviceOrientationService)
    {
        _deviceOrientationService = deviceOrientationService;
        InitializeComponent();

        MainPage = new AppShell(_deviceOrientationService);
    }
}
