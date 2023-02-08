using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface IGameHistoryPage : IPageBase<IGameHistoryPageViewModel>
{

}

public sealed partial class GameHistoryPage : IGameHistoryPage
{
	public GameHistoryPage(IGameHistoryPageViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}