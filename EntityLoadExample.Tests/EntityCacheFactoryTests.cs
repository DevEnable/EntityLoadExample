using Autofac;
using Autofac.Core;
using NSubstitute;
using Xunit;

namespace EntityLoadExample.Tests
{
    public class EntityCacheFactoryTests : ContainerTestBase
    {
        private IEntityFactory<long, MyEntity> _underlying;

        [Fact]
        public async void Should_Retrieve_Underlying_Entity()
        {
            var factory = Container.Resolve<IEntityFactory<long, MyEntity>>();

            MyEntity entity = await factory.GetEntity(5);

            Assert.Equal(5, entity.EntityId);
            await _underlying.Received(1).GetEntity(Arg.Any<long>());
        }

        [Fact]
        public async void Should_Return_Cached_Entity()
        {
            var factory = Container.Resolve<IEntityFactory<long, MyEntity>>();

            MyEntity entity = await factory.GetEntity(5);
            entity = await factory.GetEntity(5);

            Assert.Equal(5, entity.EntityId);
            // 2 calls to get the entity, but only the first actually gets the entity from the underlying repository
            await _underlying.Received(1).GetEntity(Arg.Any<long>());
        }


        protected override void SetupContainerBindings(ContainerBuilder builder)
        {
            _underlying = Substitute.For<IEntityFactory<long, MyEntity>>();
            _underlying.GetEntity(Arg.Any<long>()).Returns(ci =>
            {
                long id = ci.ArgAt<long>(0);

                return new MyEntity {EntityId = id};
            });

            builder.RegisterInstance(_underlying)
                .As<IEntityFactory<long, MyEntity>>()
                .Keyed<IEntityFactory<long, MyEntity>>("underlying");

            // This is a simpler registration than the more generic one
            // http://stackoverflow.com/questions/1189519/resolving-generic-interface-with-autofac

            builder.RegisterType<EntityCacheFactory<long, MyEntity>>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof (IEntityFactory<long, MyEntity>),
                    (pi, ctx) => ctx.ResolveKeyed<IEntityFactory<long, MyEntity>>("underlying")
                    ))
                .As<IEntityFactory<long, MyEntity>>();

        }
    }
}
