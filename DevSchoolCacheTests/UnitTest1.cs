using DevSchoolCache;
using Moq;

namespace DevSchoolCacheTests;

[TestClass]
public class UnitTest1
{
    /*private Mock<IMemoryCacheAdapter<Item>> memoryCacheMock = new();
    private Mock<IRedisAdapter<Item>> redisMock = new();
    private Mock<IRepository<Item>> repositoryMock = new();

    [TestInitialize]
    public void Initialize()
    {
        var service = new Service(memoryCacheMock.Object, redisMock.Object, repositoryMock.Object);
    }
        
    [TestMethod]
    public void GetOrAddAsync_KeyInMemoryCache_OnlyInMemoryCacheUsed()
    {
        memoryCacheMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>())).ReturnsAsync();
        /*redisMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>()));
        repositoryMock.Setup(x => x.GetOrAddAsync(It.IsAny<string>()));#1#
    }*/
    
    
}