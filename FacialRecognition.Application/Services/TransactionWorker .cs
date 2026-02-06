using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FacialRecognition.Application.Services
{
    public class TransactionWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public TransactionWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider
                        .GetRequiredService<TransactionServiceService>();

                    await service.RunTransactionCronAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
