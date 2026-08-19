namespace HandFootExcluded.UI.Services;

public interface IAlertService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");
    Task<string> ShowActionSheetAsync(string title, params string[] buttons);

    void ShowAlert(string title, string message, string cancel = "OK");
    void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No");
    void ShowActionSheet(string title, string button1, Action<bool> callback1, string button2, Action<bool> callback2);
}

internal sealed class AlertService : IAlertService
{
    private static Page CurrentPage =>
        Shell.Current?.CurrentPage
        ?? Application.Current?.Windows.FirstOrDefault()?.Page
        ?? throw new InvalidOperationException("No active page available to display alerts.");

    private static IDispatcher Dispatcher =>
        Application.Current?.Dispatcher
        ?? throw new InvalidOperationException("No dispatcher available.");

    public Task ShowAlertAsync(string title, string message, string cancel = "OK") =>
        CurrentPage.DisplayAlert(title, message, cancel);

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No") =>
        CurrentPage.DisplayAlert(title, message, accept, cancel);

    public Task<string> ShowActionSheetAsync(string title, params string[] buttons) =>
        CurrentPage.DisplayActionSheet(title, null, null, buttons);

    public void ShowAlert(string title, string message, string cancel = "OK") =>
        Dispatcher.Dispatch(async () => await ShowAlertAsync(title, message, cancel));

    public void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No")
    {
        Dispatcher.Dispatch(async () =>
        {
            var answer = await ShowConfirmationAsync(title, message, accept, cancel);
            callback(answer);
        });
    }

    public void ShowActionSheet(string title, string button1, Action<bool> callback1, string button2, Action<bool> callback2)
    {
        Dispatcher.Dispatch(async () =>
        {
            var answer = await ShowActionSheetAsync(title, button1, button2);
            if (answer == button1)
                callback1(true);
            else if (answer == button2)
                callback2(true);
        });
    }
}