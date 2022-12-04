namespace HandFootExcluded;

public partial class SettingsView : ContentView
{
	public SettingsView()
	{
        BindingContext = MauiProgram.CreateMauiApp().Services.GetService<ISettingsViewModel>();

		InitializeComponent();
	}
}