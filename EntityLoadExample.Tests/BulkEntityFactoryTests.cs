using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Xunit;

namespace EntityLoadExample.Tests
{
    public class BulkEntityFactoryTests : ContainerTestBase
    {
        [Fact]
        public async void Should_Retrieve_Entities()
        {
            var factory = Container.Resolve<IEntityFactory<long, MyEntity>>();

            List<Task<MyEntity>> retrieveTasks = new List<Task<MyEntity>>
            {
                factory.GetEntity(1),
                factory.GetEntity(2),
                factory.GetEntity(3)
            };

            await Task.WhenAll();

            Assert.Equal(1, retrieveTasks[0].Result.EntityId);
            Assert.Equal(2, retrieveTasks[1].Result.EntityId);
            Assert.Equal(3, retrieveTasks[2].Result.EntityId);
        }


        protected override void SetupContainerBindings(ContainerBuilder builder)
        {
            builder.RegisterType<MyEntityBulkFactory>().As<IEntityFactory<long, MyEntity>>();
        }
    }
}
