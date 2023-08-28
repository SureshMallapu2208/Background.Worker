using Background.Worker;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<MonitorLoop>();
        services.AddHostedService<Worker>();
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            if (!int.TryParse(context.Configuration["QueueCapacity"],
                out int queueCapacity))
            {
                queueCapacity = 1;
            }

            return new DefaultBackgroundTaskQueue(queueCapacity);
        });
    });

IHost host = builder.Build();

MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
monitorLoop.StartMonitorLoop();

host.Run();
