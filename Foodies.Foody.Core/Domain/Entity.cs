using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Foodies.Foody.Core.Domain
{
    public abstract class Entity
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        protected Entity() => Id = Guid.NewGuid().ToString();

        protected Entity(string id) => Id = id;
    }
}
