using MediatR;
using System.ComponentModel;
using X_UI_KubeView.Domain;
using Container = X_UI_KubeView.Domain.Container;

namespace X_UI_KubeView.Application.Containers;

public record GetContainersQuery : IRequest<IReadOnlyList<Container>>;

public class GetContainersQueryHandler : IRequestHandler<GetContainersQuery, IReadOnlyList<Container>>
{
    public Task<IReadOnlyList<Container>> Handle(GetContainersQuery request, CancellationToken cancellationToken)
    {
        var mockContainers = new List<Container>
        {
            new Container("1", "test-nginx", "nginx:latest", "running"),
            new Container("2", "test-redis", "redis:alpine", "exited")
        };

        return Task.FromResult<IReadOnlyList<Container>>(mockContainers);
    }
}