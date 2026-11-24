using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using X_UI_KubeView.Domain;
using X_UI_KubeView.Infrastructure;

namespace X_UI_KubeView.Desktop;

class Program
{
    public static IServiceProvider Services { get; private set; } = null!;

    public static IHostBuilder BuildHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IContainerService, DockerContainerService>();
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(X_UI_KubeView.Application.Containers.GetContainersQuery).Assembly));
                services.AddTransient<ViewModels.ContainersViewModel>();
            });
    public static int Main(string[] args)
    {
        var host = BuildHost(args).Build();
        Services = host.Services;

        return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}