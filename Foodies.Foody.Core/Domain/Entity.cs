using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Foodies.Foody.Core.Domain
{
    public abstract class Entity
    {
        public string Id { get; set; }

        protected Entity() => Id = Guid.NewGuid().ToString();

        protected Entity(string id) => Id = id;
    }

    public class GenericEntity<T> : Entity
    {
        public T Data { get; set; }

        public GenericEntity(T data) => Data = data;

        public GenericEntity()
        {
        }
    }

    public class GenericEntityBuilder
    {
        public static GenericEntity<T> Create<T>(T data) => new GenericEntity<T>(data);
    }
}
