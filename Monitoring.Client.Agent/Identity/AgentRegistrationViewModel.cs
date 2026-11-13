using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Monitoring.Client.Agent;

public sealed partial class AgentRegistrationViewModel(AgentRegistrationService registrationService) : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    public partial bool IsBusy { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    public partial bool IsRegistered { get; private set; }

    public bool CanRegister => !IsRegistered && !IsBusy;

    [RelayCommand(CanExecute = nameof(CanRegister))]
    public async Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        if (!CanRegister) return;

        try
        {
            IsBusy = true;

            await registrationService.RegisterAsync(cancellationToken);
            IsRegistered = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await Shell.Current.CurrentPage.DisplayAlertAsync("!", "Agent registration failed.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}