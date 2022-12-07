using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface ITotalScoreView : IViewBase<ITotalScoreViewModel>
{

}

public partial class TotalScoreView : ViewBase<ITotalScoreViewModel>, ITotalScoreView
{
	public TotalScoreView()
	{
		InitializeComponent();
	}
}