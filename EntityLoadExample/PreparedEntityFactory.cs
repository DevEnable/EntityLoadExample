using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public interface IEntityBuilder<TEntityData, TEntity>
    {
        TEntity BuildEntity(TEntityData data);
    }

    public interface IEntityData<TEntityIdentity, TEntityData>
        where TEntityIdentity : IEquatable<TEntityIdentity>
    {
        IDictionary<TEntityIdentity, TEntityData> GetPreparedData();
    }

    public class PreparedEntityFactory<TEntityIdentity, TEntity, TEntityData> : EntityFactoryBase<TEntityIdentity>, IEntityFactory<TEntityIdentity, TEntity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
        where TEntity : class
    {
        private readonly Lazy<IDictionary<TEntityIdentity, TEntityData>> _data;
        private readonly IEntityBuilder<TEntityData, TEntity> _builder;

        private IDictionary<TEntityIdentity, TEntityData> Data => _data.Value;


        public PreparedEntityFactory(IEntityData<TEntityIdentity, TEntityData> data, IEntityBuilder<TEntityData, TEntity> builder)
        {
            _data = new Lazy<IDictionary<TEntityIdentity, TEntityData>>(data.GetPreparedData);
            _builder = builder;
        } 

        public Task<TEntity> GetEntity(TEntityIdentity identity)
        {
            TEntityData data;

            if (Data.TryGetValue(identity, out data))
            {
                return Task.FromResult(_builder.BuildEntity(data));
            }

            return Task.FromResult<TEntity>(null);
        }
    }

    public class ThreadPreparedData<TEntityIdentity, TEntityData> : IEntityData<TEntityIdentity, TEntityData>
        where TEntityIdentity : IEquatable<TEntityIdentity>
    {
        public IDictionary<TEntityIdentity, TEntityData> GetPreparedData()
        {
            var slot = Thread.GetNamedDataSlot("prepared");

            return (IDictionary<TEntityIdentity, TEntityData>)Thread.GetData(slot);
        }
    }


    public class MyEntityBuilder : IEntityBuilder<MyEntityData, MyEntity>
    {
        public MyEntity BuildEntity(MyEntityData data)
        {
            return new MyEntity(data);
        }
    }
}
