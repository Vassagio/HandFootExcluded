using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface ISummaryPage : IPageBase<ISummaryPageViewModel> { }

public partial class SummaryPage : PageBase<ISummaryPageViewModel>, ISummaryPage
{
    public SummaryPage(ISummaryPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}