namespace X_UI_KubeView.Domain;

public interface IContainerService
{
    Task<IReadOnlyList<Container>> GetContainersAsync(CancellationToken cancellationToken);
}