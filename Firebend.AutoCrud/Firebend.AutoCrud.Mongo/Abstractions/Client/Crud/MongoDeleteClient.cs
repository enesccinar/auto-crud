using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Firebend.AutoCrud.Core.Interfaces;
using Firebend.AutoCrud.Core.Interfaces.Models;
using Firebend.AutoCrud.Mongo.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Firebend.AutoCrud.Mongo.Abstractions.Client.Crud
{
    public abstract class MongoDeleteClient<TEntity, TKey> : MongoClientBaseEntity<TKey, TEntity>, IMongoDeleteClient<TKey, TEntity>
        where TKey : struct
        where TEntity : IEntity<TKey>
    {
        protected MongoDeleteClient(IMongoClient client,
            ILogger<MongoDeleteClient<TEntity, TKey>> logger,
            IMongoEntityConfiguration<TKey, TEntity> entityConfiguration) : base(client, logger, entityConfiguration)
        {
        }

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            filter = BuildFilters(filter);
            
            var mongoCollection = GetCollection();

            var result = await RetryErrorAsync(() => mongoCollection.FindOneAndDeleteAsync(filter, null, cancellationToken));

            if (result != null)
            {
                //todo: domain events
                //await PublishDeleteAsync(result, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }
    }
}