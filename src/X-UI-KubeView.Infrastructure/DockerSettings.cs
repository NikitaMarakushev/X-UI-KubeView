namespace X_UI_KubeView.Infrastructure;

public class DockerSettings
{
    public string Endpoint { get; set; } = "npipe://./pipe/docker_engine";
}