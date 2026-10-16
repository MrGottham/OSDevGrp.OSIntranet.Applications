using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.AccessTokenExpirationResolverTests;

[TestFixture]
public class ResolveTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public void Resolve_WhenExpiresInIsPositive_ReturnsExpirationBasedOnCurrentUtcTime()
    {
        DateTimeOffset utcNow = _fixture!.Create<DateTimeOffset>();
        int expiresInSeconds = _random!.Next(1, 3601);
        _timeProviderMock!.Setup(m => m.GetUtcNow()).Returns(utcNow);

        DateTimeOffset result = AccessTokenExpirationResolver.Resolve(expiresInSeconds.ToString(), _timeProviderMock.Object);

        Assert.That(result, Is.EqualTo(utcNow.AddSeconds(expiresInSeconds)));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("invalid")]
    [TestCase("0")]
    [TestCase("-1")]
    [Category("UnitTest")]
    public void Resolve_WhenExpiresInIsMissingOrInvalid_ThrowsInvalidOperationException(string? expiresIn)
    {
        Assert.Throws<InvalidOperationException>(() => AccessTokenExpirationResolver.Resolve(expiresIn, _timeProviderMock!.Object));
    }
}