using MediatR;
using X_UI_KubeView.Domain;

namespace X_UI_KubeView.Application.Containers;

public record StartContainerCommand(string ContainerId) : IRequest<Unit>;

public class StartContainerCommandHandler : IRequestHandler<StartContainerCommand, Unit>
{
    private readonly IContainerService _service;
    public StartContainerCommandHandler(IContainerService service) => _service = service;

    public async Task<Unit> Handle(StartContainerCommand request, CancellationToken cancellationToken)
    {
        await _service.StartContainerAsync(request.ContainerId, cancellationToken);
        return Unit.Value;
    }
}

public record StopContainerCommand(string ContainerId) : IRequest<Unit>;

public class StopContainerCommandHandler : IRequestHandler<StopContainerCommand, Unit>
{
    private readonly IContainerService _service;
    public StopContainerCommandHandler(IContainerService service) => _service = service;

    public async Task<Unit> Handle(StopContainerCommand request, CancellationToken cancellationToken)
    {
        await _service.StopContainerAsync(request.ContainerId, cancellationToken);
        return Unit.Value;
    }
}