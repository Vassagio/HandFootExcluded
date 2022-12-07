using HandFootExcluded.UI.ViewModels;

namespace HandFootExcluded.UI.Views;

public interface IViewBase { }

public interface IViewBase<TViewModel> : IViewBase where TViewModel : IViewModel { }

public abstract class ViewBase<TViewModel> : ContentView, IViewBase<TViewModel> where TViewModel : IViewModel
{
    //protected ViewBase() => BindingContext = MauiProgram.Services.GetService<TViewModel>();
}