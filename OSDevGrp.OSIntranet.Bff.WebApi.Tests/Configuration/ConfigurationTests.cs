using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Configuration;

[TestFixture]
public class ConfigurationTests
{
    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithSecurityOpenIdConnectAuthority_ReturnsAuthority()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[SecurityConfigurationKeys.OpenIdConnectAuthority];

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithSecurityOpenIdConnectClientId_ReturnsClientId()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[SecurityConfigurationKeys.OpenIdConnectClientId];

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithSecurityOpenIdConnectClientSecret_ReturnsClientSecret()
    {
        IConfiguration sut = CreateSut();

        string? result = sut[SecurityConfigurationKeys.OpenIdConnectClientSecret];

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("IntegrationTest")]
    public void Configuration_WhenCalledWithSecurityTrustedDomainCollection_ReturnsTrustedDomainCollection()
    {
        IConfiguration sut = CreateSut();
        
        string? result = sut[SecurityConfigurationKeys.TrustedDomainCollection];
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    private IConfiguration CreateSut()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<ConfigurationTests>()
            .Build();
    }
}