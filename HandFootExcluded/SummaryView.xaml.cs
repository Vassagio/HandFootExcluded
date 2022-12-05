namespace HandFootExcluded;

public partial class SummaryView : ContentView
{
	

	public SummaryView()
	{
        BindingContext = MauiProgram.CreateMauiApp().Services.GetService<ISummaryViewModel>();
        
		InitializeComponent();
	}
}