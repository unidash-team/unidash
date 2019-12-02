namespace Foodies.Foody.Core.Domain
{
    public class GenericEntity<T> : Entity
    {
        public T Data { get; set; }

        public GenericEntity(T data) => Data = data;

        public GenericEntity()
        {
        }
    }
}