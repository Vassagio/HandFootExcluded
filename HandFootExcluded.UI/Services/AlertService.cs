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
    public Task ShowAlertAsync(string title, string message, string cancel = "OK") => Application.Current.MainPage.DisplayAlert(title, message, cancel);

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No") => Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);

    public Task<string> ShowActionSheetAsync(string title, params string[] buttons) => Application.Current.MainPage.DisplayActionSheet(title, null, null, buttons);

    public void ShowAlert(string title, string message, string cancel = "OK") => Application.Current.MainPage.Dispatcher.Dispatch(async () => await ShowAlertAsync(title, message, cancel));

    public void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No")
    {
        Application.Current.MainPage.Dispatcher.Dispatch(async () =>
        {
            var answer = await ShowConfirmationAsync(title, message, accept, cancel);
            callback(answer);
        });
    }

    public void ShowActionSheet(string title, string button1, Action<bool> callback1, string button2, Action<bool> callback2)
    {
        Application.Current.MainPage.Dispatcher.Dispatch(async () =>
        {
            var answer = await ShowActionSheetAsync(title, button1, button2);
            if (answer.Equals(button1)) callback1(true);
            if (answer.Equals(button2)) callback2(true);
        });
    }
}