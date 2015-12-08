using System;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public interface IEntityFactory<in TEntityIdentity, TEntity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
        where TEntity : class
    {
        Task<TEntity> GetEntity(TEntityIdentity identity);
    }
}
