using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobParser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int Interval;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            Interval = 2 * 60 * 60 * 1000;//2 Hour * 60 Min * 60 Sec * 1000Ms
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);



                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
