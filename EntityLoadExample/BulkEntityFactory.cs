using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public abstract class BulkEntityFactory<TEntityIdentity, TEntity> : EntityFactoryBase<TEntityIdentity>, IEntityFactory<TEntityIdentity, TEntity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
        where TEntity : class
    {
        // Simpleton threshold bulk entity factory, clearly not threadsafe, or logic safe.
        private const int MaximumDictionarySize = 3;
        private readonly Dictionary<TEntityIdentity, TaskCompletionSource<TEntity>> _requestedEntities = new Dictionary<TEntityIdentity, TaskCompletionSource<TEntity>>();

        public Task<TEntity> GetEntity(TEntityIdentity identity)
        {
            ValidateIdentity(identity);

            TaskCompletionSource<TEntity> completion = new TaskCompletionSource<TEntity>();

            _requestedEntities.Add(identity, completion);

            if (_requestedEntities.Count >= MaximumDictionarySize)
            {
                // TODO - Stop or record seperately subsequent entity requests
                GetEntities(_requestedEntities.Keys)
                    .ContinueWith(t =>
                    {
                        if (t.IsCanceled || t.IsFaulted)
                        {
                            foreach (var kvp in _requestedEntities)
                            {
                                kvp.Value.SetException(t.Exception);
                            }
                        }
                        else
                        {
                            Dictionary<TEntityIdentity, TEntity> retrieved = t.Result;

                            foreach (var kvp in _requestedEntities)
                            {
                                TEntity entity;
                                _requestedEntities[kvp.Key].SetResult(retrieved.TryGetValue(kvp.Key, out entity)
                                    ? entity
                                    : null);
                            }
                        }

                        _requestedEntities.Clear();

                    });

            }

            return completion.Task;
        }

        protected abstract Task<Dictionary<TEntityIdentity, TEntity>> GetEntities(IEnumerable<TEntityIdentity> identities);
    }

    public class MyEntityBulkFactory : BulkEntityFactory<long, MyEntity>
    {
        protected override Task<Dictionary<long, MyEntity>> GetEntities(IEnumerable<long> identities)
        {
            // Call database
            return Task.FromResult(identities.ToDictionary(id => id, id => new MyEntity { EntityId = id}));
        }
    }
}
