using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    private string? _errorMessage;

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
            ErrorMessage = null;

            var containers = await _mediator.Send(new GetContainersQuery());

            Containers = new ObservableCollection<Container>(containers);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка подключения к Docker: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}