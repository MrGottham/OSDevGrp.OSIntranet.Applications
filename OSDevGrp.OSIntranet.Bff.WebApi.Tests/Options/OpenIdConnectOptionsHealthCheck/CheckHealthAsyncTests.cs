using AutoFixture;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Options.OpenIdConnectOptionsHealthCheck;

[TestFixture]
public class CheckHealthAsyncTests
{
    #region Prviate variables

    private Mock<IOptions<OpenIdConnectOptions>>? _openIdConnectOptionsMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _openIdConnectOptionsMock = new Mock<IOptions<OpenIdConnectOptions>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenCalled_AssertValueWasCalledOnOpenIdConnectOptions()
    {
        IHealthCheck sut = CreateSut();

        await sut.CheckHealthAsync(CreateHealthCheckContext());

        _openIdConnectOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(openIdConnectOptions.Authority)} has not been given in the {openIdConnectOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: CreateInvalidAuthority());
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: CreateInvalidAuthority());
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: CreateInvalidAuthority());
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenAuthorityInOpenIdConnectionOptionsIsNotWellformedAbsoluteUri_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasAuthority: true, authority: CreateInvalidAuthority());
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(openIdConnectOptions.Authority)} has not been given as a wellformed absolute URI in the {openIdConnectOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientIdInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientId: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(openIdConnectOptions.ClientId)} has not been given in the {openIdConnectOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereStatusIsUnhealthy()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Unhealthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotNull()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionIsNotEmpty()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenClientSecretInOpenIdConnectOptionsIsNotSet_ReturnsHealthCheckResultWhereDescriptionSpecifiesReason()
    {
        OpenIdConnectOptions openIdConnectOptions = CreateOpenIdConnectOptions(hasClientSecret: false);
        IHealthCheck sut = CreateSut(openIdConnectOptions: openIdConnectOptions);

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.EqualTo($"{nameof(openIdConnectOptions.ClientSecret)} has not been given in the {openIdConnectOptions.GetType().Name}."));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOpenIdConnectOptionsIsValid_ReturnsHealthCheckResultWhereStatusIsHealthy()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Status, Is.EqualTo(HealthStatus.Healthy));
    }

    [Test]
    [Category("UnitTest")]
    public async Task CheckHealthAsync_WhenOpenIdConnectOptionsIsValid_ReturnsHealthCheckResultWhereDescriptionIsNull()
    {
        IHealthCheck sut = CreateSut();

        HealthCheckResult result = await sut.CheckHealthAsync(CreateHealthCheckContext());

        Assert.That(result.Description, Is.Null);
    }

    private IHealthCheck CreateSut(OpenIdConnectOptions? openIdConnectOptions = null)
    {
        _openIdConnectOptionsMock!.Setup(m => m.Value)
            .Returns(openIdConnectOptions ?? CreateOpenIdConnectOptions());

        return new WebApi.Options.OpenIdConnectOptionsHealthCheck(_openIdConnectOptionsMock.Object);
    }

    private OpenIdConnectOptions CreateOpenIdConnectOptions(bool hasAuthority = true, string? authority = null, bool hasClientId = true, string? clientId = null, bool hasClientSecret = true, string? clientSecret = null)
    {
        return new OpenIdConnectOptions
        {
            Authority = hasAuthority ? authority ?? CreateValidAuthority() : null,
            ClientId = hasClientId ? clientId ?? _fixture.Create<string>() : null,
            ClientSecret = hasClientSecret ? clientSecret ?? _fixture.Create<string>() : null
        };
    }

    private string CreateValidAuthority()
    {
        return $"https://localhost/{_fixture.Create<string>()}";
    }

    private string CreateInvalidAuthority()
    {
        return $"https://localhost:XYZ/{_fixture.Create<string>()}";
    }

    private static HealthCheckContext CreateHealthCheckContext()
    {
        return new HealthCheckContext();
    }
}