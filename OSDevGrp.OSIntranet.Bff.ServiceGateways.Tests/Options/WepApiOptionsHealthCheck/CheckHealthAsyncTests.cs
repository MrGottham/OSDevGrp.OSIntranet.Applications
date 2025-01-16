using AutoFixture;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Options.WepApiOptionsHealthCheck;

[TestFixture]
public class CheckHealthAsyncTests
{
    #region Prviate variables

    private Mock<IOptions<WebApiOptions>>? _webApiOptionsMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webApiOptionsMock = new Mock<IOptions<WebApiOptions>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertValueWasCalledOnWebApiOptions()
    {
        IHealthCheck sut = CreateSut();

        await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        _webApiOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.EqualTo($"{nameof(webApiOptions.EndpointAddress)} has not been given in the {webApiOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress: CreateInvalidEndpointAddress());
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress: CreateInvalidEndpointAddress());
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress: CreateInvalidEndpointAddress());
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenEndpointAddressInWebApiOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasEndpointAddress: true, endpointAddress: CreateInvalidEndpointAddress());
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.EqualTo($"{nameof(webApiOptions.EndpointAddress)} has not been given as a wellformed absolute URI in the {webApiOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.EqualTo($"{nameof(webApiOptions.ClientId)} has not been given in the {webApiOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInWebApiOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        WebApiOptions webApiOptions = CreateWebApiOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(webApiOptions: webApiOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.EqualTo($"{nameof(webApiOptions.ClientSecret)} has not been given in the {webApiOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenWebApiOptionsIsValid_ReturnsHealthCheckResultWhereStatusIsHealthy()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenWebApiOptionsIsValid_ReturnsHealthCheckResultWhereDescriptionIsNull()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext(), CancellationToken.None);

        Assert.That(result.Description, Is.Null);
    }

    private IHealthCheck CreateSut(WebApiOptions? webApiOptions = null)
    {
        _webApiOptionsMock!.Setup(m => m.Value)
            .Returns(webApiOptions ?? CreateWebApiOptions());

        return new ServiceGateways.Options.WepApiOptionsHealthCheck(_webApiOptionsMock.Object);
    }

    private WebApiOptions CreateWebApiOptions(bool hasEndpointAddress = true, string? endpointAddress = null, bool hasClientId = true, string? clientId = null, bool hasClientSecret = true, string? clientSecret = null)
    {
        return new WebApiOptions
        {
            EndpointAddress = hasEndpointAddress ? endpointAddress ?? CreateValidEndpointAddress() : null,
            ClientId = hasClientId ? clientId ?? _fixture.Create<string>() : null,
            ClientSecret = hasClientSecret ? clientSecret ?? _fixture.Create<string>() : null
        };
    }

    private string CreateValidEndpointAddress()
    {
        return $"https://localhost/{_fixture.Create<string>()}";
    }

    private string CreateInvalidEndpointAddress()
    {
        return $"https://localhost:XYZ/{_fixture.Create<string>()}";
    }

    private static HealthCheckContext CreateHealthCheckContext()
    {
        return new HealthCheckContext();
    }
}