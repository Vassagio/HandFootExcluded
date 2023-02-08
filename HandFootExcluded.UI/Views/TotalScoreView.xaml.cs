using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface ITotalScoreView : IViewBase<ITotalScoreViewModel>
{

}

public partial class TotalScoreView : ITotalScoreView
{
	public TotalScoreView()
	{
		InitializeComponent();
	}
}