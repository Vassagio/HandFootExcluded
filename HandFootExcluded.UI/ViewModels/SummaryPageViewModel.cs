using System.Windows.Input;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface ISummaryPageViewModel : IViewModel
{
    ICommand CloseCommand { get; }
}

internal sealed class SummaryPageViewModel : ViewModelBase, ISummaryPageViewModel
{
    private ICommand _closeCommand;

    public ICommand CloseCommand => _closeCommand ?? new Command(Close);



    private void Close()
    {
        Navigate<IGamePage>();
    }
}