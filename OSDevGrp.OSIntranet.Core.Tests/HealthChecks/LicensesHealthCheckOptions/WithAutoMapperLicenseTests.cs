using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.LicensesHealthCheckOptions;

[TestFixture]
public class WithAutoMapperLicenseTests
{
    #region Private variables

    private Mock<IConfiguration> _configurationMock;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _configurationMock = new Mock<IConfiguration>();
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNull_ThrowsArgumentNullException()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();

        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAutoMapperLicense(null));

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ParamName, Is.EqualTo("configuration"));
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsNotNull()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsSameLicensesHealthCheckOptions()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result, Is.SameAs(sut));
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsSameLicensesHealthCheckOptionsWhereConfigurationValueValidatorsIsNotNull()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result.ConfigurationValueValidators, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsSameLicensesHealthCheckOptionsWhereConfigurationValueValidatorsIsNotEmpty()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result.ConfigurationValueValidators, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsSameLicensesHealthCheckOptionsWhereOneConfigurationValueValidatorWasAddedToConfigurationValueValidators()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();
        int count = sut.ConfigurationValueValidators.Count();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result.ConfigurationValueValidators.Count(), Is.EqualTo(count + 1));
    }

    [Test]
    [Category("UnitTest")]
    public void WithAutoMapperLicense_WhenConfigurationIsNotNull_ReturnsSameLicensesHealthCheckOptionsWhereStringConfigurationValidatorWasAddedToConfigurationValueValidatorsForAutoMatterLicenseKey()
    {
        Core.HealthChecks.LicensesHealthCheckOptions sut = CreateSut();
        int count = sut.ConfigurationValueValidators.Count();

        Core.HealthChecks.LicensesHealthCheckOptions result = sut.WithAutoMapperLicense(_configurationMock.Object);

        Assert.That(result.ConfigurationValueValidators.ElementAt(count), Is.TypeOf<Core.HealthChecks.StringConfigurationValidator>());
    }

    private Core.HealthChecks.LicensesHealthCheckOptions CreateSut()
    {
        return new Core.HealthChecks.LicensesHealthCheckOptions();
    }
}