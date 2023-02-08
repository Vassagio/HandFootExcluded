using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface IRoundView : IViewBase<IRoundViewModel>
{

}

public partial class RoundView : IRoundView
{
	public RoundView() 
	{
		InitializeComponent();
	}

    protected override async void OnBindingContextChanged()
    {
        await this.TranslateTo(800, 0, 800, Easing.CubicIn);
        base.OnBindingContextChanged();
        await this.TranslateTo(0, 0, 800, Easing.CubicOut);
    }
}