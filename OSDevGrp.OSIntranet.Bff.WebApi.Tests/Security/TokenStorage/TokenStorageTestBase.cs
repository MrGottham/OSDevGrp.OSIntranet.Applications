using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenStorage;

public abstract class TokenStorageTestBase
{
    #region Methods

    protected static ICacheEntry CreateCacheEntry(Fixture fixture, Random random)
    {
        return CreateCacheEntryMock(fixture, random).Object;
    }

    protected static Mock<ICacheEntry> CreateCacheEntryMock(Fixture fixture, Random random, IToken? token = null, DateTimeOffset? absoluteExpiration = null)
    {
        Mock<ICacheEntry> cacheEntryMock = new Mock<ICacheEntry>();
        cacheEntryMock.Setup(m => m.Value)
            .Returns(token ?? fixture.CreateToken(random));
        cacheEntryMock.Setup(m => m.AbsoluteExpiration)
            .Returns(absoluteExpiration ?? DateTimeOffset.Now.AddMinutes(random.Next(5, 60)));
        return cacheEntryMock;
    }

    #endregion
}