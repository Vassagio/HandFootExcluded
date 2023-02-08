using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface ISettingsPage : IPageBase<ISettingsPageViewModel>
{

}

public sealed partial class SettingsPage : ISettingsPage
{
	public SettingsPage(ISettingsPageViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}