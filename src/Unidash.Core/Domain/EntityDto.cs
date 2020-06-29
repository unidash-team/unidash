using System;
using Unidash.Core.Domain.Flags;

namespace Unidash.Core.Domain
{
    public abstract class EntityDto : IIdentityFlag
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}