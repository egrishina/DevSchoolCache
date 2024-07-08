using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace DevSchoolCache.Tests
{
    [TestClass]
    public class HybridCacheServiceTests
    {
        private Mock<ICacheWrapper> _memoryCacheMock;
        private Mock<IRedisAdapter> _redisMock;
        private Mock<IRepository<TestEntity>> _repositoryMock;
        private IHybridCacheService<TestEntity> _service;

        [TestInitialize]
        public void Setup()
        {
            _memoryCacheMock = new Mock<ICacheWrapper>();
            _redisMock = new Mock<IRedisAdapter>();
            _repositoryMock = new Mock<IRepository<TestEntity>>();
            _service = new HybridCacheService<TestEntity>(_memoryCacheMock.Object, _redisMock.Object);
        }

        [TestMethod]
        public async Task GetOrAddAsync_ShouldReturnEntity_FromMemoryCache()
        {
            // Arrange
            var id = 1;
            var key = $"TestEntity.{id}";
            var entity = new TestEntity { Id = id };
            
            _memoryCacheMock.Setup(x => x.TryGetValue(key, out entity)).Returns(true);

            // Act
            var result = await _service.GetOrAddAsync(id);

            // Assert
            result.Should().Be(entity);
            _redisMock.Verify(x => x.TryGetValueAsync<TestEntity?>(key), Times.Never);
            _repositoryMock.Verify(x => x.TryGetById(id), Times.Never);
            _memoryCacheMock.Verify(x => x.TryGetValue(key, out entity), Times.Once);
        }

        [TestMethod]
        public async Task GetOrAddAsync_ShouldReturnEntity_FromRedisCache()
        {
            // Arrange
            var id = 1;
            var key = $"TestEntity.{id}";
            var entity = new TestEntity { Id = id };

            _memoryCacheMock.Setup(x => x.TryGetValue(key, out It.Ref<TestEntity?>.IsAny)).Returns(false);
            _redisMock.Setup(x => x.TryGetValueAsync<TestEntity?>(key)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetOrAddAsync(id);

            // Assert
            result.Should().Be(entity);
            _repositoryMock.Verify(x => x.TryGetById(id), Times.Never);
            _memoryCacheMock.Verify(x => x.TryGetValue(key, out entity), Times.Once);
        }

        [TestMethod]
        public async Task GetOrAddAsync_ShouldReturnEntity_FromRepository()
        {
            // Arrange
            var id = 1;
            var key = $"TestEntity.{id}";
            var entity = new TestEntity { Id = id };

            _memoryCacheMock.Setup(x => x.TryGetValue(key, out It.Ref<TestEntity?>.IsAny)).Returns(false);
            _redisMock.Setup(x => x.TryGetValueAsync<TestEntity?>(key)).ReturnsAsync((TestEntity?)null);
            _repositoryMock.Setup(x => x.TryGetById(id)).Returns(entity);

            // Act
            var result = await _service.GetOrAddAsync(id);

            // Assert
            result.Should().Be(entity);

            _memoryCacheMock.Verify(x => x.TryGetValue(key, out entity), Times.Once);
            _memoryCacheMock.Verify(x => x.Set(key, entity, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
            _redisMock.Verify(x => x.TryAddValueAsync(key, entity, TimeSpan.FromMinutes(5)), Times.Once);
        }

        [TestMethod]
        public async Task GetOrAddAsync_ShouldCacheNull_WhenEntityNotFound()
        {
            // Arrange
            var id = 1;
            var key = $"TestEntity.{id}";

            _memoryCacheMock.Setup(x => x.TryGetValue(key, out It.Ref<TestEntity?>.IsAny)).Returns(false);
            _redisMock.Setup(x => x.TryGetValueAsync<TestEntity?>(key)).ReturnsAsync((TestEntity?)null);
            _repositoryMock.Setup(x => x.TryGetById(id)).Returns((TestEntity?)null);

            // Act
            var result = await _service.GetOrAddAsync(id);

            // Assert
            result.Should().BeNull();
            
            _memoryCacheMock.Verify(x => x.Set(key, It.Is<TestEntity?>(e => e == null), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
            _redisMock.Verify(x => x.TryAddValueAsync(key, It.Is<TestEntity?>(e => e == null), TimeSpan.FromMinutes(5)), Times.Once);
        }
    }

    public class TestEntity
    {
        public long Id { get; set; }
    }
}
