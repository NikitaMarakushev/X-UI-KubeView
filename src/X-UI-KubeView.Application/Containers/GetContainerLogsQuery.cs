using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using X_UI_KubeView.Domain;

namespace X_UI_KubeView.Application.Containers;

public record GetContainerLogsQuery(string ContainerId) : IRequest<IAsyncEnumerable<string>>;

public class GetContainerLogsQueryHandler : IRequestHandler<GetContainerLogsQuery, IAsyncEnumerable<string>>
{
    private readonly IContainerService _containerService;

    public GetContainerLogsQueryHandler(IContainerService containerService)
    {
        _containerService = containerService;
    }

    public Task<IAsyncEnumerable<string>> Handle(GetContainerLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = _containerService.GetContainerLogsAsync(request.ContainerId, cancellationToken);
        return Task.FromResult(logs);
    }
}