using System;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public abstract class SingleEntityFactory<TEntityIdentity, TEntity> : EntityFactoryBase<TEntityIdentity>, IEntityFactory<TEntityIdentity, TEntity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
        where TEntity : class
    {
        public Task<TEntity> GetEntity(TEntityIdentity identity)
        {
            ValidateIdentity(identity);
            return GetEntityFromDatabase(identity);
        }

        protected abstract Task<TEntity> GetEntityFromDatabase(TEntityIdentity identity);
    }

    public class MyEntitySingleFactory : SingleEntityFactory<long, MyEntity>
    {
        protected override Task<MyEntity> GetEntityFromDatabase(long identity)
        {
            // TODO - Call database
            return Task.FromResult(new MyEntity {EntityId = identity});
        }
    }
}
