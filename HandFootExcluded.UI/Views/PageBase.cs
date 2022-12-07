using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;


public interface IPageBase
{

}

public interface IPageBase<TViewModel> : IPageBase where TViewModel : IViewModel
{

}

public abstract class PageBase<TViewModel> : ContentPage, IPageBase<TViewModel> where TViewModel : IViewModel
{
    protected PageBase(TViewModel viewModel)
	{
	    BindingContext = viewModel;
    }
}