using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Home.CookieConsent.CookieConsentFeature;

[TestFixture]
public class ExecuteAsyncTests
{    
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertGetUtcNowWasCalledOnTimeProvider(bool withAuthenticatedUser)
    {
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(securityContext: securityContext);
        await sut.ExecuteAsync(cookieConsentRequest);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsCookieConsentResponseWhereCookieNameIsEqualToCookieContentPrefixWithApplicationNameFromCookieConsentRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut();

        string applicationName = _fixture!.Create<string>();
        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(applicationName: applicationName, securityContext: securityContext);
        CookieConsentResponse result = await sut.ExecuteAsync(cookieConsentRequest);

        Assert.That(result.CookieName, Is.EqualTo($"{applicationName}.CookieConsent"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsCookieConsentResponseWhereCookieValueIsEqualToTrue(bool withAuthenticatedUser)
    {
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(securityContext: securityContext);
        CookieConsentResponse result = await sut.ExecuteAsync(cookieConsentRequest);

        Assert.That(result.CookieValue, Is.EqualTo("true"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsCookieConsentResponseWhereExpiresIsEqualTo90DaysFromNow(bool withAuthenticatedUser)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut(utcNow: utcNow);

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(securityContext: securityContext);
        CookieConsentResponse result = await sut.ExecuteAsync(cookieConsentRequest);

        Assert.That(result.Expires, Is.EqualTo(utcNow.AddDays(90)));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.WebsiteUsingCookies)]
    [TestCase(StaticTextKey.CookieConsentInformation)]
    [TestCase(StaticTextKey.AllowNecessaryCookies)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsCookieConsentResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: true);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(securityContext: securityContext);
        CookieConsentResponse result = await sut.ExecuteAsync(cookieConsentRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.WebsiteUsingCookies)]
    [TestCase(StaticTextKey.CookieConsentInformation)]
    [TestCase(StaticTextKey.AllowNecessaryCookies)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsCookieConsentResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<CookieConsentRequest, CookieConsentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: false);
        CookieConsentRequest cookieConsentRequest = CreateCookieConsentRequest(securityContext: securityContext);
        CookieConsentResponse result = await sut.ExecuteAsync(cookieConsentRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<CookieConsentRequest, CookieConsentResponse> CreateSut(DateTimeOffset? utcNow = null)
    {
        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Home.CookieConsent.CookieConsentFeature(_timeProviderMock!.Object, _staticTextProviderMock!.Object);
    }

    private CookieConsentRequest CreateCookieConsentRequest(string? applicationName = null, ISecurityContext? securityContext = null)
    {
        return new CookieConsentRequest(Guid.NewGuid(), applicationName ?? _fixture!.Create<string>(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext());
    }

    private ISecurityContext CreateSecurityContext(bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(withAuthenticatedUser).Object;
    }

    private ISecurityContext CreateSecurityContext(ClaimsPrincipal user)
    {
        return CreateSecurityContextMock(user).Object;
    }

    private Mock<ISecurityContext> CreateSecurityContextMock(bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(user: withAuthenticatedUser ? CreateAuthenticatedUser() : CreateNonAuthenticated());
    }

    private Mock<ISecurityContext> CreateSecurityContextMock(ClaimsPrincipal user)
    {
        return _fixture!.CreateSecurityContextMock(user: user);
    }

    private ClaimsPrincipal CreateAuthenticatedUser()
    {
        return _fixture!.CreateAuthenticatedClaimsPrincipal();
    }

    private ClaimsPrincipal CreateNonAuthenticated()
    {
        return _fixture!.CreateNonAuthenticatedClaimsPrincipal();
    }
}