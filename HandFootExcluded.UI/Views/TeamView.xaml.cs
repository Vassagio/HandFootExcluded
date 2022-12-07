using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface ITeamView : IViewBase<ITeamViewModel>
{

}

public partial class TeamView : ViewBase<ITeamViewModel>, ITeamView
{
	public TeamView() 
	{
		InitializeComponent();
	}
}