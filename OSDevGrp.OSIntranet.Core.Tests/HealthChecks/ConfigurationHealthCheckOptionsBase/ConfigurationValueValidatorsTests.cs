using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.Tests.HealthChecks.ConfigurationHealthCheckOptionsBase
{
    [TestFixture]
    public class ConfigurationValueValidatorsTests
    {
        [Test]
        [Category("UnitTest")]
        public void ConfigurationValueValidators_WhenConfigurationHealthCheckOptionsBaseIsCreated_ExpectConfigurationValueValidatorsIsNotNull()
        {
            ConfigurationHealthCheckOptionsBase<HealthCheckOptionsBase> sut = CreateSut();

            Assert.That(sut.ConfigurationValueValidators, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ConfigurationValueValidators_WhenConfigurationHealthCheckOptionsBaseIsCreated_ExpectConfigurationValueValidatorsIsEmpty()
        {
            ConfigurationHealthCheckOptionsBase<HealthCheckOptionsBase> sut = CreateSut();

            Assert.That(sut.ConfigurationValueValidators, Is.Empty);
        }

        private ConfigurationHealthCheckOptionsBase<HealthCheckOptionsBase> CreateSut()
        {
            return new Sut();
        }

        private class Sut : ConfigurationHealthCheckOptionsBase<HealthCheckOptionsBase>
        {
            #region Properties

            protected override HealthCheckOptionsBase HealthCheckOptions => this;

            #endregion
        }
    }
}