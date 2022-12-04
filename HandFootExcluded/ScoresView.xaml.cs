namespace HandFootExcluded;

public partial class ScoresView : ContentView
{
	public ScoresView()
	{
        BindingContext = MauiProgram.CreateMauiApp().Services.GetService<IScoresViewModel>();

		InitializeComponent();
	}
}