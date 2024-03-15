using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests : ConfigurationTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyKty_ReturnsKty()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyKty];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyKty_ReturnsKtyMatchingJwtKeyTypeRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyKty];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyTypeRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyN_ReturnsN()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyN];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyN_ReturnsNMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyN];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyE_ReturnsE()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyE];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyE_ReturnsEMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyE];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyD_ReturnsD()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyD];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyD_ReturnsDMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyD];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyDp_ReturnsDp()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyDp];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyDp_ReturnsDpMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyDp];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyDq_ReturnsDq()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyDq];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyDq_ReturnsDqMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyDq];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyP_ReturnsP()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyP];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyP_ReturnsPMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyP];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyQ_ReturnsQ()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyQ];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyQ_ReturnsQMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyQ];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyQi_ReturnsQi()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyQi];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtKeyQi_ReturnsQiMatchingJwtKeyBase64UrlRegularExpression()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtKeyQi];

            Assert.That(result, Is.Not.Null);
            Assert.That(ConfigurationValueRegularExpressions.JwtKeyBase64UrlRegularExpression.IsMatch(result));
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtIssuer_ReturnsIssuer()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtIssuer];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtIssuer_ReturnsIssuerMatchingAbsoluteUrl()
        {
            IConfiguration sut = CreateSut();

            bool result = Uri.TryCreate(sut[SecurityConfigurationKeys.JwtIssuer], UriKind.Absolute, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtAudience_ReturnsAudience()
        {
            IConfiguration sut = CreateSut();

            string result = sut[SecurityConfigurationKeys.JwtAudience];

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Configuration_WhenCalledWithSecurityJwtAudience_ReturnsAudienceMatchingAbsoluteUrl()
        {
            IConfiguration sut = CreateSut();

            bool result = Uri.TryCreate(sut[SecurityConfigurationKeys.JwtAudience], UriKind.Absolute, out _);

            Assert.That(result, Is.True);
        }
    }
}