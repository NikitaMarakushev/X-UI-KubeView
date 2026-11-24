using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using X_UI_KubeView.Desktop.ViewModels;

namespace X_UI_KubeView.Desktop.Views;

public partial class LogsWindow : Window
{
    private readonly CancellationTokenSource _windowCts = new();

    public LogsWindow()
    {
        InitializeComponent();
        Closed += (s, e) => _windowCts.Cancel();
    }

    public async void StartLogs(string containerId, string containerName, ContainerLogsViewModel viewModel)
    {
        DataContext = viewModel;
        await viewModel.StartLoadingLogsAsync(containerId, containerName, _windowCts.Token);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}