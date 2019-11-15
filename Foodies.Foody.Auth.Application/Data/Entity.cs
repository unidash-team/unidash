using System;

namespace Foodies.Foody.Auth.Application.Data
{
    public class Entity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}