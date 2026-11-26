namespace X_UI_KubeView.Domain;

public interface IContainerService
{
    Task<IReadOnlyList<Container>> GetContainersAsync(CancellationToken cancellationToken);

    IAsyncEnumerable<string> GetContainerLogsAsync(string containerId, CancellationToken cancellationToken);
    Task StartContainerAsync(string containerId, CancellationToken cancellationToken);
    Task StopContainerAsync(string containerId, CancellationToken cancellationToken);
}