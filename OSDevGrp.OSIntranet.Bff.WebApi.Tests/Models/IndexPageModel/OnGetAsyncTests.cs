using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Models.IndexPageModel;

[TestFixture]
public class OnGetAsyncTests
{
    #region Private variables

    private Mock<IWebHostEnvironment>? _webHostEnvironmentMock;
    private Mock<IOptions<OpenIdConnectOptions>>? _openIdConnectOptionsMock;
    private Mock<IOptions<WebApiOptions>>? _webApiOptionsMock;
    private Mock<IQueryFeature<HealthMonitorRequest, HealthMonitorResponse>>? _healthMonitorQueryFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _openIdConnectOptionsMock = new Mock<IOptions<OpenIdConnectOptions>>();
        _webApiOptionsMock = new Mock<IOptions<WebApiOptions>>();
        _healthMonitorQueryFeatureMock = new Mock<IQueryFeature<HealthMonitorRequest, HealthMonitorResponse>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertEnvironmentNameWasCalledTwiceOnWebHostEnvironment(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _webHostEnvironmentMock!.Verify(m => m.EnvironmentName, Times.Exactly(2));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertValueWasCalledOnOpenIdConnectOptions(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _openIdConnectOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertValueWasCalledOnWebApiOptions(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _webApiOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertTitleIsIntiailizedWithSpecificText(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.Title, Is.EqualTo(ProgramHelper.GetTitle()));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertDescriptionIsIntiailizedWithSpecificText(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.Description, Is.EqualTo(ProgramHelper.GetDescription()));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertOpenApiDocumentUrlIsIntiailizedWithSpecificOpenApiDocumentUrl()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentUrl, Is.EqualTo(ProgramHelper.GetOpenApiDocumentUrl(_webHostEnvironmentMock!.Object)));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertOpenApiDocumentNameIsIntiailizedWithSpecificOpenApiDocumentName()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentName, Is.EqualTo(ProgramHelper.GetOpenApiDocumentName()));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertOpenApiDocumentUrlIsNotIntiailized()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentUrl, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertOpenApiDocumentNameIsNotIntiailized()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.OpenApiDocumentName, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenUserInHttpContextHasNoIdentity_AsserAuthenticatedUserIsNotInitialized(bool isDevelopment)
    {
        ClaimsPrincipal user = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: false);
        HttpContext httpContext = CreateHttpContext(user: user);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, httpContext: httpContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.AuthenticatedUser, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenUserInHttpContextHasNonAuthenticatedIdentity_AsserAuthenticatedUserIsNotInitialized(bool isDevelopment)
    {
        ClaimsPrincipal user = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: true);
        HttpContext httpContext = CreateHttpContext(user: user);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, httpContext: httpContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.AuthenticatedUser, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenUserInHttpContextHasAuthenticatedIdentity_AsserAuthenticatedUserIsInitializedWithIdentityFromUserInHttpContext(bool isDevelopment)
    {
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        HttpContext httpContext = CreateHttpContext(user: user);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, httpContext: httpContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.AuthenticatedUser, Is.EqualTo(claimsIdentity));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertAuthenticationEnabledIsIntiailizedWithTrue()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.AuthenticationEnabled, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsDevelopment_AssertReturnUrlIsIntiailizedWithUrlForHttpRequestFromHttpContext()
    {
        Uri requestUri = CreateRequestUri();
        HttpContext httpContext = CreateHttpContext(requestUri: requestUri);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: true, httpContext: httpContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.ReturnUrl, Is.EqualTo(requestUri));
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertAuthenticationEnabledIsIntiailizedWithFalse()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.AuthenticationEnabled, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task OnGetAsync_WhenEnviromentIsNotDevelopment_AssertReturnUrlIsNotIntiailized()
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: false);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.ReturnUrl, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertExecuteAsyncWasCalledOnHealthMonitorQueryFeatureWithHealthMonitorRequestWhereDependenciesContainsTwoDependencies(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _healthMonitorQueryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<HealthMonitorRequest>(value => value.Dependencies.Count() == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenAuthorityIsNotSetInOpenIdConnectOptions_ThrowsInvalidOperationException(bool isDevelopment)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenAuthorityIsNotSetInOpenIdConnectOptions_ThrowsInvalidOperationExceptionWhereErrorMessageIsSpecificText(bool isDevelopment)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("The authority '' is not a valid absolute URI."));
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenAuthorityIsNotSetInOpenIdConnectOptions_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull(bool isDevelopment)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenAuthorityIsNoneAbsoluteUriInOpenIdConnectOptions_ThrowsInvalidOperationException(bool isDevelopment, string authority)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: authority);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenAuthorityIsNoneAbsoluteUriInOpenIdConnectOptions_ThrowsInvalidOperationExceptionWhereErrorMessageIsSpecificText(bool isDevelopment, string authority)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: authority);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Is.EqualTo($"The authority '{authority}' is not a valid absolute URI."));
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenAuthorityIsNoneAbsoluteUriInOpenIdConnectOptions_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull(bool isDevelopment, string authority)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: authority);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "https://api.localhost", "https://api.localhost/health")]
    [TestCase(true, "https://api.localhost/", "https://api.localhost/health")]
    [TestCase(false, "https://api.localhost", "https://api.localhost/health")]
    [TestCase(false, "https://api.localhost/", "https://api.localhost/health")]
    public async Task OnGetAsync_WhenAuthorityIsAbsoluteUriInInOpenIdConnectOptions_AssertExecuteAsyncWasCalledOnHealthMonitorQueryFeatureWithHealthMonitorRequestWhereDependenciesContainsDependencyforOpenIdConnect(bool isDevelopment, string authority, string expectedHealthEndpointAddress)
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: authority);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, openIdConnectOptions: openIdConnectOptions);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _healthMonitorQueryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<HealthMonitorRequest>(value => value.Dependencies.SingleOrDefault(dependency => dependency.Description == "OpenID Connect Authority"  && dependency.HealthEndpoint.AbsoluteUri == expectedHealthEndpointAddress) != null),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenEndpointAddressIsNotSetInWebApiOptions_ThrowsInvalidOperationException(bool isDevelopment)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenEndpointAddressIsNotSetInWebApiOptions_ThrowsInvalidOperationExceptionWhereErrorMessageIsSpecificText(bool isDevelopment)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("The endpoint address '' is not a valid absolute URI."));
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenEndpointAddressIsNotSetInWebApiOptions_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull(bool isDevelopment)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenEndpointAddressIsNoneAbsoluteUriInWebApiOptions_ThrowsInvalidOperationException(bool isDevelopment, string endpointAddress)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenEndpointAddressIsNoneAbsoluteUriInWebApiOptions_ThrowsInvalidOperationExceptionWhereErrorMessageIsSpecificText(bool isDevelopment, string endpointAddress)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Is.EqualTo($"The endpoint address '{endpointAddress}' is not a valid absolute URI."));
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "XYZ")]
    [TestCase(true, "123")]
    [TestCase(false, "XYZ")]
    [TestCase(false, "123")]
    public async Task OnGetAsync_WhenEndpointAddressIsNoneAbsoluteUriInWebApiOptions_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull(bool isDevelopment, string endpointAddress)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        try
        {
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await sut.OnGetAsync(cancellationTokenSource.Token);

            Assert.Fail("An InvalidOperationException was expected, but no exception was thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, "https://api.localhost", "https://api.localhost/health")]
    [TestCase(true, "https://api.localhost/", "https://api.localhost/health")]
    [TestCase(false, "https://api.localhost", "https://api.localhost/health")]
    [TestCase(false, "https://api.localhost/", "https://api.localhost/health")]
    public async Task OnGetAsync_WhenEndpointAddressIsAbsoluteUriInWebApiOptions_AssertExecuteAsyncWasCalledOnHealthMonitorQueryFeatureWithHealthMonitorRequestWhereDependenciesContainsDependencyforWebApi(bool isDevelopment, string endpointAddress, string expectedHealthEndpointAddress)
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, webApiOptions: webApiOptions);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _healthMonitorQueryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<HealthMonitorRequest>(value => value.Dependencies.SingleOrDefault(dependency => dependency.Description == "OS Development Group Web API"  && dependency.HealthEndpoint.AbsoluteUri == expectedHealthEndpointAddress) != null),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertExecuteAsyncWasCalledOnHealthMonitorQueryFeatureWithGivenCancellationToken(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.OnGetAsync(cancellationToken);

        _healthMonitorQueryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<HealthMonitorRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertDependenciesIsIntiailizedWithDependenciesFromResolvedHealthMonitorResponse(bool isDevelopment)
    {
        DependencyHealthResultModel[] dependencies = CreateDependencyHealthResultModelCollection();
        HealthMonitorResponse healthMonitorResponse = CreateHealthMonitorResponse(dependencies: dependencies);
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment, healthMonitorResponse: healthMonitorResponse);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(sut.Dependencies, Is.EqualTo(dependencies));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_ReturnsNotNull(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_ReturnsPageResult(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.OnGetAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<PageResult>());
    }

    private WebApi.Models.IndexPageModel CreateSut(bool isDevelopment = true, OpenIdConnectOptions? openIdConnectOptions = null, WebApiOptions? webApiOptions = null, HttpContext? httpContext = null, HealthMonitorResponse? healthMonitorResponse = null)
    {
        _webHostEnvironmentMock!.Setup(m => m.EnvironmentName)
            .Returns(isDevelopment ? "Development" : "Production");

        _openIdConnectOptionsMock!.Setup(m => m.Value)
            .Returns(openIdConnectOptions ?? CreateOpenIdConnectOptions());

        _webApiOptionsMock!.Setup(m => m.Value)
            .Returns(webApiOptions ?? CreateWebApiOptions());

        _healthMonitorQueryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<HealthMonitorRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(healthMonitorResponse ?? CreateHealthMonitorResponse()));

        return new WebApi.Models.IndexPageModel(_webHostEnvironmentMock!.Object, _openIdConnectOptionsMock.Object, _webApiOptionsMock!.Object, _healthMonitorQueryFeatureMock.Object)
        {
            PageContext = new PageContext
            {
                HttpContext = httpContext ?? CreateHttpContext()
            }
        };
    }

    private OpenIdConnectOptions CreateOpenIdConnectOptions(bool hasAuthority = true, string? authority = null)
    {
        return new OpenIdConnectOptions
        {
            Authority = hasAuthority ? authority ?? CreateEndpointAddress() : null
        };
    }

    private WebApiOptions CreateWebApiOptions(bool hasEndpointAddress = true, string? endpointAddress = null)
    {
        return new WebApiOptions
        {
            EndpointAddress = hasEndpointAddress ? endpointAddress ?? CreateEndpointAddress() : null
        };
    }

    private string CreateEndpointAddress()
    {
        return $"https://{_fixture!.Create<string>()}.local";
    }

    private HttpContext CreateHttpContext(Uri? requestUri = null, ClaimsPrincipal? user = null)
    {
        requestUri ??= CreateRequestUri();

        return new DefaultHttpContext
        {
            Request =
            {
                Scheme = requestUri.Scheme,
                Host = HostString.FromUriComponent(requestUri),
                PathBase = PathString.Empty,
                Path = PathString.FromUriComponent(requestUri),
                QueryString = QueryString.FromUriComponent(requestUri)
            },
            User = user ?? _fixture!.CreateAuthenticatedClaimsPrincipal()
        };
    }

    private Uri CreateRequestUri()
    {
        return new Uri($"https://{_fixture!.Create<string>()}.local/{_fixture!.Create<string>()}", UriKind.Absolute);
    }

    private HealthMonitorResponse CreateHealthMonitorResponse(IEnumerable<DependencyHealthResultModel>? dependencies = null)
    {
        return new HealthMonitorResponse(dependencies ?? CreateDependencyHealthResultModelCollection());
    }

    private DependencyHealthModel CreateDependencyHealthModel()
    {
        return new DependencyHealthModel(_fixture!.Create<string>(), new Uri($"{CreateEndpointAddress()}/health", UriKind.Absolute));
    }

    private DependencyHealthResultModel[] CreateDependencyHealthResultModelCollection()
    {
        return
        [
            CreateDependencyHealthResultModel(),
            CreateDependencyHealthResultModel(),
            CreateDependencyHealthResultModel()
        ];
    }

    private DependencyHealthResultModel CreateDependencyHealthResultModel()
    {
        return CreateDependencyHealthModel().GenerateResult(_fixture!.Create<bool>());
    }
}