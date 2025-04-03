using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Local.HealthMonitor;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Models.IndexPageModel;

[TestFixture]
public class OnGetAsyncTests
{
    #region Private variables

    private Mock<IWebHostEnvironment>? _webHostEnvironmentMock;
    private Mock<IOptions<WebApiOptions>>? _webApiOptionsMock;
    private Mock<IQueryFeature<HealthMonitorRequest, HealthMonitorResponse>>? _healthMonitorQueryFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _webApiOptionsMock = new Mock<IOptions<WebApiOptions>>();
        _healthMonitorQueryFeatureMock = new Mock<IQueryFeature<HealthMonitorRequest, HealthMonitorResponse>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task OnGetAsync_WhenCalled_AssertEnvironmentNameWasCalledOnWebHostEnvironment(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _webHostEnvironmentMock!.Verify(m => m.EnvironmentName, Times.Once);
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
    public async Task OnGetAsync_WhenCalled_AssertExecuteAsyncWasCalledOnHealthMonitorQueryFeatureWithHealthMonitorRequestWhereDependenciesContainsOneDependency(bool isDevelopment)
    {
        WebApi.Models.IndexPageModel sut = CreateSut(isDevelopment: isDevelopment);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.OnGetAsync(cancellationTokenSource.Token);

        _healthMonitorQueryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<HealthMonitorRequest>(value => value.Dependencies.Count() == 1),
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

    private WebApi.Models.IndexPageModel CreateSut(bool isDevelopment = true, WebApiOptions? webApiOptions = null, HealthMonitorResponse? healthMonitorResponse = null)
    {
        _webHostEnvironmentMock!.Setup(m => m.EnvironmentName)
            .Returns(isDevelopment ? "Development" : "Production");

        _webApiOptionsMock!.Setup(m => m.Value)
            .Returns(webApiOptions ?? CreateWebApiOptions());

        _healthMonitorQueryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<HealthMonitorRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(healthMonitorResponse ?? CreateHealthMonitorResponse()));

        return new WebApi.Models.IndexPageModel(_webHostEnvironmentMock!.Object, _webApiOptionsMock!.Object, _healthMonitorQueryFeatureMock.Object);
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