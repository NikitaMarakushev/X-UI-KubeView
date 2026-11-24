using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using X_UI_KubeView.Domain;

namespace X_UI_KubeView.Infrastructure;

public class DockerContainerService : IContainerService
{
    private readonly DockerClient _dockerClient;

    public DockerContainerService()
    {
        var config = new DockerClientConfiguration(
            new Uri("tcp://localhost:2375"));

        _dockerClient = config.CreateClient();
    }

    public async Task<IReadOnlyList<Container>> GetContainersAsync(CancellationToken cancellationToken)
    {
        var containersResponse = await _dockerClient.Containers.ListContainersAsync(
            new ContainersListParameters { All = true },
            cancellationToken);

        return containersResponse.Select(c => new Container(
            Id: c.ID.Length > 12 ? c.ID.Substring(0, 12) : c.ID,
            Name: c.Names.FirstOrDefault()?.TrimStart('/') ?? "unknown",
            Image: c.Image,
            Status: c.State
        )).ToList().AsReadOnly();
    }
}