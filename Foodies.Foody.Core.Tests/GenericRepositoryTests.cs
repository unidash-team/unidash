using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Foodies.Foody.Core.Domain;
using Foodies.Foody.Core.Infrastructure;
using Xunit;

namespace Foodies.Foody.Core.Tests
{
    public class GenericRepositoryTests
    {
        [Fact]
        public async Task Add_SingleItem_ShouldFind()
        {
            // Arrange
            var repository = CreateTestRepository();

            // Act
            Entity entity = await repository.AddAsync(new GenericEntity<string>("The cake is a lie"));
            await repository.AddAsync(new GenericEntity<string>("Cupcakes and chocolate"));

            // Assert
            Assert.NotNull(await repository.FindByIdAsync(entity.Id));
        }

        [Fact]
        public async Task Remove_ExistingItem_Removes()
        {
            // Arrange
            var repository = CreateTestRepository();

            // Act
            var entity = await repository.AddAsync(new GenericEntity<string>("Cookies"));
            await repository.RemoveAsync(entity);

            // Assert
            Assert.Null(await repository.FindByIdAsync(entity.Id));
        }

        [Fact]
        public async Task CreateWhenNotFound_SingleItem_ShouldCreate()
        {
            // Arrange
            var repository = CreateTestRepository();

            // Act
            var entity = GenericEntityBuilder.Create("Chocolate");
            entity.Id = "choco";

            Assert.Null(await repository.FindByIdAsync(entity.Id));
            var newEntity = await repository.GetOrCreateAsync(entity.Id, entity);

            // Assert
            Assert.NotNull(newEntity);
            Assert.Equal(entity.Id, newEntity.Id);
        }

        [Fact]
        public async Task Add_Entity_PersistsId()
        {
            // Arrange
            var repository = CreateTestRepository();
            var entity = GenericEntityBuilder.Create("Nuts");
            entity.Id = "123";

            // Act
            var addedEntity = await repository.AddAsync(entity);

            // Assert
            Assert.Equal(entity.Id, addedEntity.Id);
        }

        [Fact]
        public async Task FindAll_AddedEntities_ReturnsCorrectCount()
        {
            // Arrange 
            var repository = CreateTestRepository();
            var entityIceCream = GenericEntityBuilder.Create("Ice Cream");
            var entityChocoCake = GenericEntityBuilder.Create("Choco Cake");

            // Act
            await repository.AddAsync(entityIceCream);
            await repository.AddAsync(entityChocoCake);

            // Assert
            var list = await repository.FindAllAsync();
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task FindById_NotExistingItem_ReturnsNull()
        {
            // Arrange
            var repository = CreateTestRepository();
            
            // Act
            var entity = await repository.FindByIdAsync("12345");

            // Assert
            Assert.Null(entity);
        }

        // Seedwork
        private IEntityRepository<GenericEntity<string>> CreateTestRepository()
        {
            return new InMemoryEntityRepository<GenericEntity<string>>();
        }
    }
}
