using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;

[TestFixture]
public class RemoveVerificationCodeAsyncTests
{
    #region Private variables

    private Mock<IMemoryCache>? _memoryCacheMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task RemoveVerificationCodeAsync_WhenCalled_AssertRemoveWasCalledOnMemoryCacheWithGivenVerificationKey()
    {
        IVerificationCodeStorage sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.RemoveVerificationCodeAsync(verificationKey, cancellationTokenSource.Token);

        _memoryCacheMock!.Verify(m => m.Remove(It.Is<string>(value => value == verificationKey)), Times.Once);
    }

    private IVerificationCodeStorage CreateSut(ICacheEntry? createdCacheEntry = null)
    {
        return new DomainServices.Security.VerificationCodeStorage(_memoryCacheMock!.Object);
    }
}