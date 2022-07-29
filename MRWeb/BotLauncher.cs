using BotManager.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MRWeb
{
    public class BotLauncher : IHostedService
    {
        private readonly IConfiguration configuration;

        public BotLauncher(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await BotClient.Create(configuration);
            //ReviewersList.Create(Enumerable.Empty<Reviewer>());
            ReviewersList.Create();
            GroupList.Create();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
