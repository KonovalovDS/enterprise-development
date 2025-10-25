namespace Library.DataGenerator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly BogusGenerator _generator;

    public Worker(ILogger<Worker> logger, BogusGenerator generator)
    {
        _logger = logger;
        _generator = generator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
