namespace Foodies.Foody.Core.Domain
{
    public class GenericEntityBuilder
    {
        public static GenericEntity<T> Create<T>(T data) => new GenericEntity<T>(data);
    }
}