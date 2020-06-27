using System;
using Unidash.Core.Domain.Flags;

namespace Unidash.Core.Domain
{
    public abstract class Entity : IIdentityFlag
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        protected Entity() => Id = Guid.NewGuid().ToString();

        protected Entity(string id) => Id = id;
    }
}
