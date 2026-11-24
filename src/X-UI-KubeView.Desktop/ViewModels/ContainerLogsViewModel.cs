using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Docker.DotNet.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using X_UI_KubeView.Application.Containers;

namespace X_UI_KubeView.Desktop.ViewModels;

public partial class ContainerLogsViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string _containerName = string.Empty;

    [ObservableProperty]
    private string _logText = "Инициализация...";

    [ObservableProperty]
    private bool _isLoading;

    public ContainerLogsViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task StartLoadingLogsAsync(string containerId, string containerName, CancellationToken externalToken)
    {
        ContainerName = $"Логи контейнера: {containerName}";
        LogText = "Подключение к потоку логов...\n";
        IsLoading = true;

        _cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);

        try
        {
            var query = new GetContainerLogsQuery(containerId);
            var logLines = new Queue<string>();
            const int maxLines = 500;

            var logStream = await _mediator.Send(query, _cts.Token);

            await foreach (var line in logStream)
            {
                logLines.Enqueue(line);
                if (logLines.Count > maxLines)
                {
                    logLines.Dequeue();
                }

                LogText = string.Join(Environment.NewLine, logLines);
            }
        }
        catch (TaskCanceledException)
        {
            LogText += "\n\n[Чтение логов остановлено пользователем]";
        }
        catch (Exception ex)
        {
            LogText += $"\n\n[Ошибка чтения: {ex.Message}]";
        }
        finally
        {
            IsLoading = false;
            _cts?.Dispose();
        }
    }

    [RelayCommand]
    private void StopLogs()
    {
        _cts?.Cancel();
    }
}