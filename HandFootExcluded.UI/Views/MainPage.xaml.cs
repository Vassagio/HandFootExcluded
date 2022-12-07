using HandFootExcluded.UI.ViewModels;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI;

public interface IMainPage : IPageBase<IMainPageViewModel> { }

public sealed partial class MainPage : PageBase<IMainPageViewModel>, IMainPage
{
    public MainPage(IMainPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}

