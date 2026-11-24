using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using X_UI_KubeView.Application.Containers;
using X_UI_KubeView.Domain;
using Container = X_UI_KubeView.Domain.Container;

namespace X_UI_KubeView.Desktop.ViewModels;

public partial class ContainersViewModel : ObservableObject
{
    private readonly IMediator _mediator;

    [ObservableProperty]
    private ObservableCollection<Container> _containers = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _isDataVisible;

    public ContainersViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task LoadContainersAsync()
    {
        try
        {
            IsLoading = true;
            HasError = false;
            ErrorMessage = null;
            IsDataVisible = false;

            var containers = await _mediator.Send(new GetContainersQuery());
            Containers = new ObservableCollection<Container>(containers);

            HasError = containers.Count == 0;
            ErrorMessage = containers.Count == 0 ? "Контейнеры не найдены. Убедитесь, что Docker запущен." : null;
            IsDataVisible = containers.Count > 0;
        }
        catch (Exception ex)
        {
            HasError = true;
            IsDataVisible = false;
            ErrorMessage = $"Ошибка подключения к Docker. Убедитесь, что WSL запущен и Docker доступен.\nДетали: {ex.Message}";
            Containers.Clear();
        }
        finally
        {
            IsLoading = false;
        }
    }
}