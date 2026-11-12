using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Monitoring.Identity.Contracts;

namespace Monitoring.Client.Admin;

public sealed partial class UserRowViewModel : ObservableObject
{
    public required User UserContract { get; init; }
    public required AdminApiService AdminApi { private get; init; }

    public DateTime LocalFirstSeenAt => UserContract.OnlineDetails.FirstSeenAt.LocalDateTime;
    public DateTime LocalLastHeartbeatAt => UserContract.OnlineDetails.LastHeartbeatAt.LocalDateTime;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RequestScreenshotCommand))]
    public partial bool IsBusy { get; private set; }

    public bool CanExecute => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task RequestScreenshotAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IsBusy = true;
            await AdminApi.RequestScreenshotAsync(UserContract.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            await Shell.Current.CurrentPage.DisplayAlertAsync("!", ex.ToString(), "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}