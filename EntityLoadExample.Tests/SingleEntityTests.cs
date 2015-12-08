using Autofac;
using Xunit;

namespace EntityLoadExample.Tests
{
    public class SingleEntityTests : ContainerTestBase
    {
        [Fact]
        public async void Should_Load_Entity()
        {
            var result = await Container.Resolve<IEntityFactory<long, MyEntity>>().GetEntity(5);
            Assert.Equal(5, result.EntityId);
        }

        protected override void SetupContainerBindings(ContainerBuilder builder)
        {
            builder.RegisterType<MyEntitySingleFactory>().As<IEntityFactory<long, MyEntity>>();
        }
    }
}
