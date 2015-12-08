using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace EntityLoadExample.Tests
{
    public abstract class ContainerTestBase
    {
        protected IContainer Container { get; private set; }

        protected ContainerTestBase()
        {
            ContainerBuilder builder = new ContainerBuilder();
            // ReSharper disable once VirtualMemberCallInContructor
            SetupContainerBindings(builder);
            Container = builder.Build();
        }

        protected abstract void SetupContainerBindings(ContainerBuilder builder);

    }
}
