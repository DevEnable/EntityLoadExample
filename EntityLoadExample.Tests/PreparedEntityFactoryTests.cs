using System.Collections.Generic;
using System.Threading;
using Autofac;
using Xunit;

namespace EntityLoadExample.Tests
{
    public class PreparedEntityFactoryTests : ContainerTestBase
    {
        [Fact]
        public async void Should_Use_Prepared()
        {
            // Data previously prepared.
            var slot = Thread.AllocateNamedDataSlot("prepared");
            Thread.SetData(slot, new Dictionary<long, MyEntityData>
            {
                {5, new MyEntityData {EntityId = 5} }
            });

            // Retrieving the data later

            var factory = Container.Resolve<IEntityFactory<long, MyEntity>>();
            MyEntity entity = await factory.GetEntity(5);

            Assert.Equal(5, entity.EntityId);
        }

        protected override void SetupContainerBindings(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof (ThreadPreparedData<,>))
                .As(typeof (IEntityData<,>));

            builder.RegisterType<MyEntityBuilder>().As<IEntityBuilder<MyEntityData, MyEntity>>().SingleInstance();

            // Can't use generic registration as we have different generic type arguments.
            builder.RegisterType<PreparedEntityFactory<long, MyEntity, MyEntityData>>()
                .As<IEntityFactory<long, MyEntity>>();
        }
    }
}
