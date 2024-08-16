using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationFactory
{
    [TestFixture]
    public class CreateTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenIssuerIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(null, _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("issuer"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenAuthorizationEndpointIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), null, _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizationEndpoint"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenTokenEndpointIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), null, _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenEndpoint"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenJsonWebKeySetEndpointIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() =>
                Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), null, CreateStringArray(), CreateStringArray(), CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("jsonWebKeySetEndpoint"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenResponseTypesSupportedIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), null, CreateStringArray(), CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("responseTypesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenSubjectTypesSupported_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), null, CreateStringArray()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("subjectTypesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenIdTokenSigningAlgValuesSupported_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("idTokenSigningAlgValuesSupported"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder result = Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), CreateStringArray());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsOpenIdProviderConfigurationBuilder()
        {
            IOpenIdProviderConfigurationBuilder result = Domain.Security.OpenIdProviderConfigurationFactory.Create(_fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), _fixture.CreateEndpoint(), CreateStringArray(), CreateStringArray(), CreateStringArray());

            Assert.That(result, Is.TypeOf<Domain.Security.OpenIdProviderConfigurationBuilder>());
        }

        private string[] CreateStringArray()
        {
            return _fixture.CreateMany<string>(_random.Next(1, 10)).ToArray();
        }
    }
}