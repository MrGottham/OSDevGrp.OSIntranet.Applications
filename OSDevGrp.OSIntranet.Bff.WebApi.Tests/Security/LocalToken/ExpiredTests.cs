using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.LocalToken;

[TestFixture]
public class ExpiredTests
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
    public void Expired_WhenExpiresIsLocalTime_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        IToken sut = CreateSut(expires: DateTimeOffset.Now.AddMinutes(_random!.Next(5, 60)));

        bool _ = sut.Expired;

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsLocalTimeLaterThanUtcNow_ReturnsFalse()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow.ToLocalTime().AddMilliseconds(1), utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsLocalTimeEqualToUtcNow_ReturnsTrue()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow.ToLocalTime(), utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsLocalTimeEarlierThanUtcNow_ReturnsTrue()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow.ToLocalTime().AddMilliseconds(-1), utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsUtcTime_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        IToken sut = CreateSut(expires: DateTimeOffset.UtcNow.AddMinutes(_random!.Next(5, 60)));

        bool _ = sut.Expired;

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsUtcTimeLaterThanUtcNow_ReturnsFalse()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow.AddMilliseconds(1), utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsUtcTimeEqualToUtcNow_ReturnsTrue()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow, utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void Expired_WhenExpiresIsUtcTimeEarlierThanUtcNow_ReturnsTrue()
    {
        DateTime utcNow = DateTime.UtcNow;
        IToken sut = CreateSut(expires: utcNow.AddMilliseconds(-1), utcNow: utcNow);

        bool result = sut.Expired;

        Assert.That(result, Is.True);
    }

    private IToken CreateSut(DateTimeOffset? expires = null, DateTimeOffset? utcNow = null)
    {
        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        return new WebApi.Security.LocalToken(_fixture!.Create<string>(), _fixture!.Create<string>(), expires ?? DateTimeOffset.UtcNow.AddMilliseconds(_random!.Next(5, 60)), _timeProviderMock!.Object);
    }
}