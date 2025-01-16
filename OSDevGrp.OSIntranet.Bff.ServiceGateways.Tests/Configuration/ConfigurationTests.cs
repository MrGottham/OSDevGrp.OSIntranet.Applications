using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Configuration;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Configuration;

[TestFixture]
public class ConfigurationTests : ServiceGatewayTestBase
{
    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiEndpointAddressKey_ReturnsNotNull()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiEndpointAddressKey];

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiEndpointAddressKey_ReturnsNotEmpty()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiEndpointAddressKey];

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiEndpointAddressKey_ReturnsWellFormedAbsoluteUriString()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiEndpointAddressKey];

        Assert.That(Uri.IsWellFormedUriString(result, UriKind.Absolute), Is.True);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiClientIdKey_ReturnsNotNull()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiClientIdKey];

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiClientIdKey_ReturnsNotEmpty()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiClientIdKey];

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiClientSecretKey_ReturnsNotNull()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiClientSecretKey];

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithServiceGatewaysWebApiClientSecretKey_ReturnsNotEmpty()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[ConfigurationKeys.ServiceGatewaysWebApiClientSecretKey];

        Assert.That(result, Is.Not.Empty);
    }

    private IConfiguration CreateSut()
    {
        return CreateTestConfiguration();
    }
}