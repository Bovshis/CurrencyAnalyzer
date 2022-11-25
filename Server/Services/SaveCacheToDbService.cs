using Microsoft.Extensions.Caching.Memory;
using Server.Data;

namespace Server.Services
{
    public class SaveCacheToDbService : IHostedService, IDisposable
    {
        private readonly MongoRepository _mongoRepository;
        private Timer? _timer = null;
        private const int period = 5;

        public SaveCacheToDbService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(period));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            Console.WriteLine("Save to db");
            _mongoRepository.SaveDate();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
