using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.LinkModels
{
    public class ShapedEntity
    {
        public ShapedEntity()
        {
            Entity = new Entity();
        }

        public Guid Id { get; set; }
        public Entity Entity { get; set; }
    }

}
