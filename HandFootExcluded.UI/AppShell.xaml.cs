using HandFootExcluded.UI.Services;
using HandFootExcluded.UI.ViewModels;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI;

public partial class AppShell : Shell
{
    private readonly IDeviceOrientationService _deviceOrientationService;

    public AppShell(IDeviceOrientationService deviceOrientationService)
    {
        _deviceOrientationService = deviceOrientationService ?? throw new ArgumentNullException(nameof(deviceOrientationService));

        InitializeComponent();
        
        MainContent.Content = MauiProgram.Services.GetService<IMainPage>();
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
