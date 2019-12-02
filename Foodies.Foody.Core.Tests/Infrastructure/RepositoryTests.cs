using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Foodies.Foody.Core.Domain;
using Foodies.Foody.Core.Infrastructure;
using Foodies.Foody.Core.Tests.SeedWork;
using Xunit;

namespace Foodies.Foody.Core.Tests.Infrastructure
{
    /// <summary>
    /// This test data class returns all possible repositories so we don't
    /// need to create separate tests for each implementation of <see cref="IEntityRepository{T}"/>.
    ///
    /// Reference: https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
    /// </summary>
    public class RepositoryTestData<T> : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new InMemoryEntityRepository<GenericEntity<T>>() };
            yield return new object[] { new MongoEntityRepository<GenericEntity<T>>("mongodb://localhost:27018", "testing") };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RepositoryTests : IClassFixture<Mongo2GoDatabaseFixture>
    {
        private readonly Mongo2GoDatabaseFixture _databaseFixture;

        public RepositoryTests(Mongo2GoDatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task Add_SingleItem_ShouldFind(IEntityRepository<GenericEntity<string>> repository)
        {
            // Act
            Entity entity = await repository.AddAsync(new GenericEntity<string>("The cake is a lie"));
            await repository.AddAsync(new GenericEntity<string>("Cupcakes and chocolate"));
            var match = await repository.FindByIdAsync(entity.Id);

            // Assert
            match.Should().NotBeNull("because we added an entity before");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task Remove_ExistingItem_Removes(IEntityRepository<GenericEntity<string>> repository)
        {
            // Act
            var entity = await repository.AddAsync(new GenericEntity<string>("Cookies"));
            await repository.RemoveAsync(entity);
            var match = await repository.FindByIdAsync(entity.Id);

            // Assert
            match.Should().BeNull("because it was removed from the repository");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task CreateWhenNotFound_SingleItem_ShouldCreate(IEntityRepository<GenericEntity<string>> repository)
        {
            // Act
            var entity = GenericEntityBuilder.Create("Chocolate");
            entity.Id = "choco";

            Assert.Null(await repository.FindByIdAsync(entity.Id));
            var newEntity = await repository.GetOrCreateAsync(entity.Id, entity);

            // Assert
            newEntity
                .Should()
                .NotBeNull("because it exists in the repository");

            entity.Id
                .Should()
                .Be(newEntity.Id,
                    "because it was not found and then added with the same ID");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task Add_Entity_PersistsId(IEntityRepository<GenericEntity<string>> repository)
        {
            // Arrange
            var entity = GenericEntityBuilder.Create("Nuts");
            entity.Id = "123";

            // Act
            var addedEntity = await repository.AddAsync(entity);

            // Assert
            entity.Id
                .Should()
                .Be(addedEntity.Id, "because we added it before with this ID");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task FindAll_AddedEntities_ReturnsCorrectCount(IEntityRepository<GenericEntity<string>> repository)
        {
            // Arrange 
            var entityIceCream = GenericEntityBuilder.Create("Ice Cream");
            var entityChocoCake = GenericEntityBuilder.Create("Choco Cake");

            // Act
            await repository.AddAsync(entityIceCream);
            await repository.AddAsync(entityChocoCake);
            var list = (await repository.FindAllAsync()).ToList();

            // Assert
            list.Should().HaveCount(2, "because we added two items");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<string>))]
        public async Task FindById_NotExistingItem_ReturnsNull(IEntityRepository<GenericEntity<string>> repository)
        {
            // Act
            var entity = await repository.FindByIdAsync("12345");

            // Assert
            entity.Should().BeNull("because we didn't add it to the repository");
        }

        [Theory]
        [ClassData(typeof(RepositoryTestData<KeyValuePair<string, string>>))]
        public async Task FindByLinq_ExistingItem_Successfully(IEntityRepository<GenericEntity<KeyValuePair<string, string>>> repository)
        {
            // Arrange
            const string expectedKey = "kitchen@foody.pro";

            await repository.AddAsync(
                new GenericEntity<KeyValuePair<string, string>>(
                    new KeyValuePair<string, string>(expectedKey, "foody")));

            // Act
            var entity = (await repository.FindAllByAsync(x => x.Data.Key == expectedKey))
                .First();

            // Assert
            entity.Data.Key
                .Should()
                .Be(expectedKey, 
                    "because it exists in the repository with the same key");
        }

        // Seedwork
        private IEntityRepository<GenericEntity<T>> CreateTestRepository<T>() => 
            new InMemoryEntityRepository<GenericEntity<T>>();

        private IEntityRepository<GenericEntity<string>> CreateTestRepository() => 
            CreateTestRepository<string>();
    }
}
