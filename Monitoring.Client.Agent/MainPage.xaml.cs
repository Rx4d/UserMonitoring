namespace Monitoring.Client.Agent;

public sealed partial class MainPage : ContentPage
{
    public MainPage(AgentRegistrationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AgentRegistrationViewModel viewModel) Dispatcher.DispatchAsync(() => viewModel.RegisterAsync());
    }
}