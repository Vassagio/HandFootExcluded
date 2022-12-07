using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface IRoundView : IViewBase<IRoundViewModel>
{

}

public partial class RoundView : ViewBase<IRoundViewModel>, IRoundView
{
	public RoundView() 
	{
		InitializeComponent();
	}
}