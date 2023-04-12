using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Repositories.Interfaces.Configuration;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Configuration
{
	[TestFixture]
    public class ConfigurationTests : ConfigurationTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientId_ReturnsClientId()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.MicrosoftClientId];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftClientSecret_ReturnsClientSecret()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.MicrosoftClientSecret];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityMicrosoftTenant_ReturnsTenant()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.MicrosoftTenant];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithExternalDataDashboardEndpointAddress_ReturnsEndpointAddress()
        {
            IConfiguration sut = CreateSut();

            string result = sut[ExternalDataConfigurationKeys.DashboardEndpointAddress];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithTestDataAccountingExistingAccountingNumber_ReturnsExistingAccountingNumber()
        {
            IConfiguration sut = CreateSut();

            string result = sut["TestData:Accounting:ExistingAccountingNumber"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            Assert.That(int.Parse(result), Is.GreaterThan(0));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithTestDataAccountingExistingAccountNumberForAccount_ReturnsExistingAccountNumberForAccount()
        {
            IConfiguration sut = CreateSut();

            string result = sut["TestData:Accounting:ExistingAccountNumberForAccount"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithTestDataAccountingExistingAccountNumberForBudgetAccount_ReturnsExistingAccountNumberForBudgetAccount()
        {
            IConfiguration sut = CreateSut();

            string result = sut["TestData:Accounting:ExistingAccountNumberForBudgetAccount"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithTestDataAccountingExistingAccountNumberForContactAccount_ReturnsExistingAccountNumberForContactAccount()
        {
            IConfiguration sut = CreateSut();

            string result = sut["TestData:Accounting:ExistingAccountNumberForContactAccount"];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithTestDataMediaLibraryExistingMediaPersonalityIdentifier_ReturnsExistingMediaPersonalityIdentifier()
        {
	        IConfiguration sut = CreateSut();

	        string result = sut["TestData:MediaLibrary:ExistingMediaPersonalityIdentifier"];

	        Assert.That(result, Is.Not.Null);
	        Assert.That(result, Is.Not.Empty);
            Assert.That(Guid.TryParse(result, out Guid _), Is.True);
        }
    }
}