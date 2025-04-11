using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DependencyHealth;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Local;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DependencyHealth.DependencyHealthMonitor;

[TestFixture]
public class CheckHealthAsyncTests
{
    #region Private variables

    private Mock<IHttpClientFactory>? _httpClientFactoryMock;
    private Mock<HttpClient>? _httpClientMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpClientMock = new Mock<HttpClient>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertCreateClientWasCalledOnHttpClientFactory()
    {
        IDependencyHealthMonitor sut = CreateSut();

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        await sut.CheckHealthAsync(dependencyHealthModel);

        _httpClientFactoryMock!.Verify(m => m.CreateClient(It.Is<string>(value => value == $"{typeof(DomainServices.Logic.DependencyHealth.DependencyHealthMonitor).Namespace}.{typeof(DomainServices.Logic.DependencyHealth.DependencyHealthMonitor).Name}:Client")), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertSendAsyncWasCalledOnHttpClientCreatedByHttpClientFactoryWithHttpRequestMessageWhereMethodIsGet()
    {
        IDependencyHealthMonitor sut = CreateSut();

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        await sut.CheckHealthAsync(dependencyHealthModel);

        _httpClientMock!.Verify(m => m.SendAsync(
                It.Is<HttpRequestMessage>(value => value.Method == HttpMethod.Get),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertSendAsyncWasCalledOnHttpClientCreatedByHttpClientFactoryWithHttpRequestMessageWhereRequestUriIsEqualToHealthEndpointFromGivenDependencyHealthModel()
    {
        IDependencyHealthMonitor sut = CreateSut();

        Uri healthEndpoint = CreateHealthEndpoint();
        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel(healthEndpoint: healthEndpoint);
        await sut.CheckHealthAsync(dependencyHealthModel);

        _httpClientMock!.Verify(m => m.SendAsync(
                It.Is<HttpRequestMessage>(value => value.RequestUri == healthEndpoint),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertSendAsyncWasCalledOnHttpClientCreatedByHttpClientFactoryWithHttpRequestMessageWhereContentIsNull()
    {
        IDependencyHealthMonitor sut = CreateSut();

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        await sut.CheckHealthAsync(dependencyHealthModel);

        _httpClientMock!.Verify(m => m.SendAsync(
                It.Is<HttpRequestMessage>(value => value.Content == null),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertSendAsyncWasCalledOnHttpClientCreatedByHttpClientFactoryWithGivenCancellationToken()
    {
        IDependencyHealthMonitor sut = CreateSut();

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CheckHealthAsync(dependencyHealthModel, cancellationToken);

        _httpClientMock!.Verify(m => m.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.OK)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task CheckHealthAsync_WhenCalled_ReturnsDependencyHealthResultModelWhereDescriptionIsEqualToDescriptionFromGivenDependencyHealthModel(HttpStatusCode httpStatusCode)
    {
        IDependencyHealthMonitor sut = CreateSut(httpStatusCode: httpStatusCode);

        string description = _fixture!.Create<string>();
        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel(description: description);
        DependencyHealthResultModel result = await sut.CheckHealthAsync(dependencyHealthModel);

        Assert.That(result.Description, Is.EqualTo(description));

    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.OK)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task CheckHealthAsync_WhenCalled_ReturnsDependencyHealthResultModelWhereHealthEndpointIsEqualToHealthEndpointFromGivenDependencyHealthModel(HttpStatusCode httpStatusCode)
    {
        IDependencyHealthMonitor sut = CreateSut(httpStatusCode: httpStatusCode);

        Uri healthEndpoint = CreateHealthEndpoint();
        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel(healthEndpoint: healthEndpoint);
        DependencyHealthResultModel result = await sut.CheckHealthAsync(dependencyHealthModel);

        Assert.That(result.HealthEndpoint, Is.EqualTo(healthEndpoint));

    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.OK, true)]
    [TestCase(HttpStatusCode.ServiceUnavailable, false)]
    [TestCase(HttpStatusCode.InternalServerError, false)]
    public async Task CheckHealthAsync_WhenCalled_ReturnsDependencyHealthResultModelWhereIsHealthyBasedOnHttpStatusCodeFromHttpResponseMessage(HttpStatusCode httpStatusCode, bool isHealty)
    {
        IDependencyHealthMonitor sut = CreateSut(httpStatusCode: httpStatusCode);

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        DependencyHealthResultModel result = await sut.CheckHealthAsync(dependencyHealthModel);

        Assert.That(result.IsHealthy, Is.EqualTo(isHealty));

    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenHttpRequestExceptionIsThrown_ReturnsDependencyHealthResultModelWhereIsHealthyIsFalse()
    {
        HttpRequestException exception = new HttpRequestException(_fixture!.Create<string>());
        IDependencyHealthMonitor sut = CreateSut(exception: exception);

        DependencyHealthModel dependencyHealthModel = CreateDependencyHealthModel();
        DependencyHealthResultModel result = await sut.CheckHealthAsync(dependencyHealthModel);

        Assert.That(result.IsHealthy, Is.False);
    }

    private IDependencyHealthMonitor CreateSut(HttpStatusCode? httpStatusCode = null, Exception? exception = null)
    {
        _httpClientFactoryMock!.Setup(m => m.CreateClient(It.IsAny<string>()))
            .Returns(_httpClientMock!.Object);

        if (exception == null)
        {
            _httpClientMock!.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpResponseMessage(httpStatusCode ?? HttpStatusCode.OK)));
        }
        else
        {
            _httpClientMock!.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }

        return new DomainServices.Logic.DependencyHealth.DependencyHealthMonitor(_httpClientFactoryMock!.Object);
    }

    private DependencyHealthModel CreateDependencyHealthModel(string? description = null, Uri? healthEndpoint = null)
    {
        return new DependencyHealthModel(description ?? _fixture!.Create<string>(), healthEndpoint ?? CreateHealthEndpoint());
    }

    private Uri CreateHealthEndpoint()
    {
        return new Uri($"http://{_fixture!.Create<string>()}.local/{_fixture!.Create<string>()}");
    }
}