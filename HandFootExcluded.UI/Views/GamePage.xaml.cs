using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface IGamePage : IPageBase<IGamePageViewModel>
{

}

public partial class GamePage : PageBase<IGamePageViewModel>, IGamePage
{
	public GamePage(IGamePageViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}