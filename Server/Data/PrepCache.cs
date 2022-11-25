using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Data
{
    public static class PrepCache
    {
        public static void PrepCurrencyRates(IApplicationBuilder app)
        {
            var mongoRepository = app.ApplicationServices.GetService<MongoRepository>();
            if (mongoRepository == null) return;
            mongoRepository.LoadDate();
        }
    }
}
