namespace Monitoring.Client.Admin;

public sealed partial class MainPage : ContentPage
{
    public MainPage(UserTableViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserTableViewModel viewModel) Dispatcher.DispatchAsync(() => viewModel.RefreshAsync());
    }
}