using AutoFixture;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Options.TrustedDomainOptionsHealthCheck;

[TestFixture]
public class CheckHealthAsyncTests
{
    #region Prviate variables

    private Mock<IOptions<TrustedDomainOptions>>? _trustedDomainOptionsMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _trustedDomainOptionsMock = new Mock<IOptions<TrustedDomainOptions>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertValueWasCalledOnTrustedDomainOptions()
    {
        IHealthCheck sut = CreateSut();

        await sut.CheckHealthAsync(CreateHealthCheckContext());

        _trustedDomainOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: false);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: false);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: false);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: false);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(trustedDomainOptions.TrustedDomainCollection)} has not been given in the {trustedDomainOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: true, trustedDomains: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: true, trustedDomains: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: true, trustedDomains: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainCollectionInTrustedDomainOptionsIsSetButDoesNotContainAnyValues_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(hasTrustedDomainCollection: true, trustedDomains: [string.Empty, " ", "  ", "   "]);
        IHealthCheck sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(trustedDomainOptions.TrustedDomainCollection)} does not contain any values in the {trustedDomainOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainOptionsIsValid_ReturnsHealthCheckResultWhereStatusIsHealthy()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenTrustedDomainOptionsIsValid_ReturnsHealthCheckResultWhereDescriptionIsNull()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Null);
    }

    private IHealthCheck CreateSut(TrustedDomainOptions? trustedDomainOptions = null)
    {
        _trustedDomainOptionsMock!.Setup(m => m.Value)
            .Returns(trustedDomainOptions ?? CreateTrustedDomainOptions());

        return new WebApi.Options.TrustedDomainOptionsHealthCheck(_trustedDomainOptionsMock.Object);
    }

    private TrustedDomainOptions CreateTrustedDomainOptions(bool hasTrustedDomainCollection = true, IEnumerable<string>? trustedDomains = null)
    {
        return new TrustedDomainOptions
        {
            TrustedDomainCollection = hasTrustedDomainCollection ? string.Join(';', trustedDomains ?? _fixture!.CreateMany<string>(_random!.Next(5, 10))) : null,
        };
    }

    private static HealthCheckContext CreateHealthCheckContext()
    {
        return new HealthCheckContext();
    }
}