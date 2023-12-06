using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcache.CACHE
{
    public class StateSaver : IHostedService, IDisposable
    {
        private readonly IClusterClient clusterClient;
        private readonly ILogger<StateSaver> logger;
        private Timer timer;

        public StateSaver(IClusterClient clusterClient, ILogger<StateSaver> logger)
        {
            this.clusterClient = clusterClient;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Adjust the interval as needed
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                // Get all grains of the specified type
                var grainReferences = await clusterClient.GetGrainDirectory().GetGrainTypeInstances<IYourGrain>();

                // Save the state of each grain
                foreach (var grainReference in grainReferences)
                {
                    var grain = clusterClient.GetGrain<IYourGrain>(grainReference.PrimaryKey);
                    await grain.WriteStateAsync();
                }

                logger.LogInformation("Grain states saved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving grain states.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
