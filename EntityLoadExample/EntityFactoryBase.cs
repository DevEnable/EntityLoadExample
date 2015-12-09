using System;
using System.Collections.Generic;

namespace EntityLoadExample
{
    public abstract class EntityFactoryBase<TEntityIdentity>
        where TEntityIdentity : IEquatable<TEntityIdentity>
    {
        private readonly EqualityComparer<TEntityIdentity> _defaultComparer = EqualityComparer<TEntityIdentity>.Default;

        protected void ValidateIdentity(TEntityIdentity identity)
        {
            if (_defaultComparer.Equals(identity, default(TEntityIdentity)))
            {
                throw new ArgumentException("Identity cannot be null or empty", "identity");
            }
        }
    }
}
