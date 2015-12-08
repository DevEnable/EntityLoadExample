using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLoadExample
{
    public class MyEntityData
    {
        public long EntityId { get; set; }
    }

    public class MyEntity
    {
        public long EntityId { get; set; }

        public MyEntity()
        { }

        public MyEntity(MyEntityData data)
        {
            EntityId = data.EntityId;
        }


        public decimal ReallyComplexLogic()
        {
            return EntityId;
        }

    }
}
