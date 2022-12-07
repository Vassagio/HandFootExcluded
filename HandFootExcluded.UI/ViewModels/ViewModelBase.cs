using HandFootExcluded.Common;
using HandFootExcluded.UI.Views;

namespace HandFootExcluded.UI.ViewModels;

public interface IViewModel : IBindableItem { }

internal abstract class ViewModelBase : BindableItem
{
    protected static void Navigate<TPage>() where TPage : IPageBase
    {
        var page = MauiProgram.Services.GetService<TPage>();
        if (page is ContentPage contentPage)
            Application.Current.MainPage = contentPage;
    }
}