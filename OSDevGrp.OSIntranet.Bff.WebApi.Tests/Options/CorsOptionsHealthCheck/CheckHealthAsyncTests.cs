using AutoFixture;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Options.CorsOptionsHealthCheck;

[TestFixture]
public class CheckHealthAsyncTests
{
    #region Prviate variables

    private Mock<IOptions<CorsOptions>>? _corsOptionsMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _corsOptionsMock = new Mock<IOptions<CorsOptions>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertValueWasCalledOnCorsOptions()
    {
        IHealthCheck sut = CreateSut();

        await sut.CheckHealthAsync(CreateHealthCheckContext());

        _corsOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: false);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: false);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: false);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: false);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(corsOptions.OriginCollection)} has not been given in the {corsOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: true, origins: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: true, origins: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: true, origins: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOriginCollectionInCorsOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        CorsOptions corsOptions = CreateCorsOptions(hasOriginCollection: true, origins: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(corsOptions: corsOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(corsOptions.OriginCollection)} does not contain any values in the {corsOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCorsOptionsIsValid_ReturnsHealthCheckResultWhereStatusIsHealthy()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCorsOptionsIsValid_ReturnsHealthCheckResultWhereDescriptionIsNull()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Null);
    }

    private IHealthCheck CreateSut(CorsOptions? corsOptions = null)
    {
        _corsOptionsMock!.Setup(m => m.Value)
            .Returns(corsOptions ?? CreateCorsOptions());

        return new WebApi.Options.CorsOptionsHealthCheck(_corsOptionsMock.Object);
    }

    private CorsOptions CreateCorsOptions(bool hasOriginCollection = true, IEnumerable<string>? origins = null)
    {
        return new CorsOptions
        {
            OriginCollection = hasOriginCollection ? string.Join(';', origins ?? _fixture!.CreateMany<string>(_random!.Next(5, 10))) : null,
        };
    }

    private static HealthCheckContext CreateHealthCheckContext()
    {
        return new HealthCheckContext();
    }
}