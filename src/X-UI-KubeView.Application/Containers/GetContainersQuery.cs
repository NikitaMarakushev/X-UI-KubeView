using MediatR;
using X_UI_KubeView.Domain;
using Container = X_UI_KubeView.Domain.Container;

namespace X_UI_KubeView.Application.Containers;

public record GetContainersQuery : IRequest<IReadOnlyList<Container>>;

public class GetContainersQueryHandler : IRequestHandler<GetContainersQuery, IReadOnlyList<Container>>
{
    private readonly IContainerService _containerService;

    public GetContainersQueryHandler(IContainerService containerService)
    {
        _containerService = containerService;
    }

    public async Task<IReadOnlyList<Container>> Handle(
        GetContainersQuery request,
        CancellationToken cancellationToken)
    {
        return await _containerService.GetContainersAsync(cancellationToken);
    }
}