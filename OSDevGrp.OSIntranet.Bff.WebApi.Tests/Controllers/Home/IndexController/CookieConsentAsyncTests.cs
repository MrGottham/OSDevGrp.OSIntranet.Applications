using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Home.IndexController;

[TestFixture]
public class CookieConsentAsyncTests
{
    #region Private variables

    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<CookieConsentRequest, CookieConsentResponse>>? _queryFeatureMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<CookieConsentRequest, CookieConsentResponse>>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
        _random = new Random();
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithCookieConsentRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<CookieConsentRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithCookieConsentRequestWhereApplicationNameIsEqualToGivenApplicationName()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        string applicationName = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, applicationName, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<CookieConsentRequest>(value => value.ApplicationName == applicationName),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithCookieConsentRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Home.HomeController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<CookieConsentRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithCookieConsentRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(_random!);
        WebApi.Controllers.Home.HomeController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<CookieConsentRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<CookieConsentRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_AssertGetUtcNowWasCalledOnTimeProviderProvidedByCookieConsentResponse()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationToken);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);

    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CookieConsentAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsCookieConsentResponseDto()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult) await sut.CookieConsentAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<CookieConsentResponseDto>());
    }

    private WebApi.Controllers.Home.HomeController CreateSut(IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, DateTimeOffset? utcNow = null, CookieConsentResponse? cookieConsentResponse = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, _random!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<CookieConsentRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(cookieConsentResponse ?? CreateCookieConsentResponse()));

        return new WebApi.Controllers.Home.HomeController(formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private CookieConsentResponse CreateCookieConsentResponse()
    {
        Dictionary<StaticTextKey, string> staticTexts = new Dictionary<StaticTextKey, string>
        {
            { StaticTextKey.WebsiteUsingCookies, _fixture!.Create<string>() },
            { StaticTextKey.CookieConsentInformation, _fixture.Create<string>() },
            { StaticTextKey.AllowNecessaryCookies, _fixture.Create<string>() }
        };

        return new CookieConsentResponse(_fixture.Create<string>(), _fixture.Create<string>(), DateTime.Now.AddDays(_random!.Next(30, 90)), _timeProviderMock!.Object, staticTexts);
    }
}