﻿namespace HandFootExcluded;

public partial class MainPage : ContentPage
{
    private int count = 0;

    public MainPage()
    {
        BindingContext = MauiProgram.CreateMauiApp().Services.GetService<IMainPageViewModel>();

        InitializeComponent();
    }
}
