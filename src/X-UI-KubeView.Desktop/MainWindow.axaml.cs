using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using X_UI_KubeView.Desktop.ViewModels;

namespace X_UI_KubeView.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var viewModel = Program.Services.GetRequiredService<ContainersViewModel>();
        DataContext = viewModel;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (DataContext is ContainersViewModel vm)
        {
            _ = vm.LoadContainersCommand.ExecuteAsync(null);
        }
    }
}