using System;
using System.Threading.Tasks;
using Firebend.AutoCrud.Mongo.Interfaces;

namespace Firebend.AutoCrud.Mongo.Abstractions.Client
{
    public class MongoRetryService : IMongoRetryService
    {
        public async Task<TReturn> RetryErrorAsync<TReturn>(Func<Task<TReturn>> method, int maxTries)
        {
            var tries = 0;
            double delay = 100;
            TimeSpan delayTimeSpan;

            while (true)
            {
                try
                {
                    return await method();
                }
                catch
                {
                    tries++;

                    if (tries >= maxTries)
                    {
                        throw;
                    }

                    if (tries != 1)
                    {
                        delay = Math.Ceiling(Math.Pow(delay, 1.1));
                    }

                    delayTimeSpan = TimeSpan.FromMilliseconds(delay);
                    await Task.Delay(delayTimeSpan);
                }
            }
        }
    }
}
