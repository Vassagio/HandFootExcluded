using HandFootExcluded.Common;
using HandFootExcluded.UI.Services;
using HandFootExcluded.UI.ViewModels;
using HandFootExcluded.UI.Views;
using HandFootExcluded.Core;
using HandFootExcluded.UI.Services.GameHistoryServices;
using HandFootExcluded.UI.Services.ScoringServices;

namespace HandFootExcluded.UI;

public static class MauiProgram
{
    public static IServiceProvider Services;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
           .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
           .Services
           .AddServices()
           .AddViewModels()
           .AddViews();
        
        var result = builder.Build();
        
        Services = result.Services; 
        
        return result;
    }

    

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<IAlertService, AlertService>()
                         .AddSingleton<IDeviceOrientationService, DeviceOrientationService>()
                         .AddSingleton<IScoringService, ScoringService>()
                         .AddSingleton<IScoreLineFactory, ScoreLineFactory>()
                         .AddSingleton(SecureStorage.Default)
                         .AddSingleton<IGameHistoryService, GameHistoryService>()
                         .AddExtension<CoreDependencies>();

    private static IServiceCollection AddViewModels(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<IMainPageViewModel, MainPageViewModel>()
                         .AddSingleton<ISettingsPageViewModel, SettingsPageViewModel>()
                         .AddSingleton<IGamePageViewModel, GamePageViewModel>()
                         .AddSingleton<IGameHistoryPageViewModel, GameHistoryPageViewModel>()
                         .AddSingleton<IRoundViewModel, RoundViewModel>()
                         .AddSingleton<ITeamViewModel, TeamViewModel>()
                         .AddSingleton<ITotalScoreViewModel, TotalScoreViewModel>()
                         .AddSingleton<ISummaryPageViewModel, SummaryPageViewModel>();

    private static IServiceCollection AddViews(this IServiceCollection serviceCollection) =>
        serviceCollection.AddSingleton<IMainPage, MainPage>()
                         .AddSingleton<ISettingsPage, SettingsPage>()
                         .AddSingleton<IGamePage, GamePage>()
                         .AddSingleton<IGameHistoryPage, GameHistoryPage>()
                         .AddSingleton<IRoundView, RoundView>()
                         .AddSingleton<ITeamView, TeamView>()
                         .AddSingleton<ITotalScoreView, TotalScoreView>()
                         .AddSingleton<ISummaryPage, SummaryPage>();
}
