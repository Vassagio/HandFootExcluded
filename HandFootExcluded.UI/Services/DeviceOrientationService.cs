namespace HandFootExcluded.UI.Services;

public interface IDeviceOrientationService
{
    void SetOrientation();
    void RestSetOrientation();
}

public sealed class DeviceOrientationService : IDeviceOrientationService
{
    public void SetOrientation()
    {
#if ANDROID
        Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Locked;
        Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
#endif
    }

    public void RestSetOrientation()
    {
#if ANDROID
        Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.User;
#endif
    }
}