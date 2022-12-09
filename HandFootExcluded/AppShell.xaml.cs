using HandFootExcluded.Services;

namespace HandFootExcluded;

public partial class AppShell : Shell
{
    private readonly IDeviceOrientationService _deviceOrientationService;

    public AppShell(IDeviceOrientationService deviceOrientationService)
    {
        _deviceOrientationService = deviceOrientationService ?? throw new ArgumentNullException(nameof(deviceOrientationService));

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        _deviceOrientationService.SetOrientation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing(); 

        _deviceOrientationService.RestSetOrientation();
    }
}