using System;

namespace Unidash.Core.Domain
{
    public abstract class EntityDto
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}