﻿namespace HandFootExcluded;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
           .UseMauiApp<App>()
           .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IPlayerBuilder, PlayerBuilder>();
        builder.Services.AddSingleton<IGameService, GameService>();
        builder.Services.AddSingleton<IScoringService, ScoringService>();
        builder.Services.AddSingleton<IMainPageViewModel, MainPageViewModel>();
        builder.Services.AddSingleton<ISettingsViewModel, SettingsViewModel>();

        return builder.Build();
    }
}