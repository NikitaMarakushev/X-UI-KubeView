using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Options;
using X_UI_KubeView.Domain;

namespace X_UI_KubeView.Infrastructure;

public class DockerContainerService : IContainerService
{
    private readonly DockerClient _dockerClient;

    public DockerContainerService(IOptions<DockerSettings> settings)
    {
        var uri = new Uri("tcp://localhost:2375");
        var config = new DockerClientConfiguration(uri);

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

    public async IAsyncEnumerable<string> GetContainerLogsAsync(
        string containerId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var parameters = new ContainerLogsParameters
        {
            ShowStdout = true,
            ShowStderr = true,
            Follow = true,
            Tail = "100"
        };

        using var stream = await _dockerClient.Containers.GetContainerLogsAsync(
            containerId,
            false,
            parameters,
            cancellationToken);

        var buffer = new byte[8192];
        var lineBuffer = new StringBuilder();

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await stream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken);

            if (result.Count == 0)
            {
                if (lineBuffer.Length > 0)
                {
                    yield return lineBuffer.ToString();
                }
                break;
            }

            string chunk = Encoding.UTF8.GetString(buffer, 0, result.Count);
            lineBuffer.Append(chunk);

            while (lineBuffer.ToString().Contains('\n'))
            {
                int newlineIndex = lineBuffer.ToString().IndexOf('\n');
                string line = lineBuffer.ToString().Substring(0, newlineIndex).TrimEnd('\r');
                lineBuffer.Remove(0, newlineIndex + 1);

                yield return line;
            }
        }
    }
}