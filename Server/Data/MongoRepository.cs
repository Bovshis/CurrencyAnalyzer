using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using Server.Models;
using System.Reflection;
using Server.Services;

namespace Server.Data
{
    public class MongoRepository
    {
        private readonly IMemoryCache _memoryCache;
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DbName = "CurrencyAnalyzer";
        private const string TableName = "CurrencyRates";
        private readonly IMongoDatabase _db;  

        public MongoRepository(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            var client = new MongoClient(ConnectionString);
            _db = client.GetDatabase(DbName);
        }

        public void LoadDate()
        {
            var records = _db
                .GetCollection<CurrencyRateRecord>(TableName)
                .Find(_ => true)
                .ToList();
            foreach (var record in records)
            {
                var currencyRate = new CurrencyRate()
                {
                    Currency = record.Currency,
                    Date = record.Date,
                    Value = record.Value,
                    Amount = record.Amount,

                };
                _memoryCache.Set(record.CurrencyId, currencyRate);
            }
        }

        public void SaveDate()
        {
            foreach (var key in _memoryCache.GetKeys())
            {
                var isAvailable = _memoryCache.TryGetValue(key, out CurrencyRate? currencyRate);
                if (!isAvailable) continue;
                InsertRecord(currencyRate);
            }
        }

        private void InsertRecord(CurrencyRate? currencyRate)
        {
            if (currencyRate == null) return;
            var currencyRateRecord = new CurrencyRateRecord()
            {
                CurrencyId = $"{currencyRate.Currency}&{currencyRate.Date:yyyy-MM-dd}",
                Amount = currencyRate.Amount,
                Currency = currencyRate.Currency,
                Date = currencyRate.Date,
                Value = currencyRate.Value,
            };

            var collection = _db.GetCollection<CurrencyRateRecord>(TableName);
            collection.ReplaceOne(
                r => r.CurrencyId == currencyRateRecord.CurrencyId,
                currencyRateRecord,
                new ReplaceOptions(){IsUpsert = true});
        }
    }
}
