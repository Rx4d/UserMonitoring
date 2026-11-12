namespace Monitoring.Client.Admin;

public sealed partial class UserScreenshotPage : ContentPage
{
    public UserScreenshotPage(UserScreenshotViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}