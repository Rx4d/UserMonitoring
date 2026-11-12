using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Monitoring.Client.Admin;

public sealed partial class UserTableViewModel : ObservableObject
{
    private readonly AdminApiService _adminApi;

    public UserTableViewModel(AdminApiService adminApi)
    {
        _adminApi = adminApi;
    }

    public ObservableCollection<UserRowViewModel> Users { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    public partial bool IsBusy { get; private set; }

    public bool CanExecute => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanExecute))]
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        if (!CanExecute) return;

        try
        {
            IsBusy = true;

            var users = (await _adminApi.GetUsersAsync(cancellationToken)).Select(x => new UserRowViewModel()
            {
                UserContract = x,
                AdminApi = _adminApi
            });

            Users.Clear();
            foreach (var user in users) Users.Add(user);
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