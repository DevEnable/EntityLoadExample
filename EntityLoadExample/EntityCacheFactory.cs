using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public class EntityCacheFactory<TEntityIdentity, TEntity> : EntityFactoryBase<TEntityIdentity>, IEntityFactory<TEntityIdentity, TEntity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
        where TEntity : class
    {
        private readonly IEntityFactory<TEntityIdentity, TEntity> _missFactory; 
        
        private readonly Dictionary<TEntityIdentity, TEntity> _cacheHandler = new Dictionary<TEntityIdentity, TEntity>(); 

        public EntityCacheFactory(IEntityFactory<TEntityIdentity, TEntity> missFactory)
        {
            _missFactory = missFactory;
        } 

        public async Task<TEntity> GetEntity(TEntityIdentity identity)
        {
            ValidateIdentity(identity);

            TEntity entity;

            if (_cacheHandler.TryGetValue(identity, out entity))
            {
                return entity;
            }

            entity = await _missFactory.GetEntity(identity);

            if (entity != null)
            {
                _cacheHandler.Add(identity, entity);
            }

            return entity;
        }
    }
}
