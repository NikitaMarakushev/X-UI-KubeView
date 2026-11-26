using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using X_UI_KubeView.Application.Containers;
using X_UI_KubeView.Domain;
using Xunit;

using Container = X_UI_KubeView.Domain.Container;

namespace X_UI_KubeView.Application.Tests;

public class GetContainersQueryHandlerTests
{
    private readonly IContainerService _containerService;
    private readonly GetContainersQueryHandler _handler;

    public GetContainersQueryHandlerTests()
    {
        _containerService = Substitute.For<IContainerService>();

        _handler = new GetContainersQueryHandler(_containerService);
    }

    [Fact]
    public async Task Handle_WhenContainersExist_ReturnsCorrectList()
    {
        var fakeContainers = new List<Container>
        {
            new Container("1234567890ab", "my-nginx", "nginx:latest", "running"),
            new Container("abcdef123456", "my-redis", "redis:alpine", "exited")
        }.AsReadOnly();

        _containerService.GetContainersAsync(Arg.Any<CancellationToken>())
            .Returns(fakeContainers);

        var result = await _handler.Handle(new GetContainersQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(fakeContainers);
        result.Should().HaveCount(2);

        await _containerService.Received(1).GetContainersAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenNoContainers_ReturnsEmptyList()
    {
        var emptyList = new List<Container>().AsReadOnly();

        _containerService.GetContainersAsync(Arg.Any<CancellationToken>())
            .Returns(emptyList);

        var result = await _handler.Handle(new GetContainersQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}