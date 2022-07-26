using System;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TrustedDomainHelper
{
    [TestFixture]
    public class IsTrustedDomainTests
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenUriIsNull_ThrowsArgumentNullException()
        {
            ITrustedDomainHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsTrustedDomain(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("uri"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenUriIsTrustedDomain_ReturnsTrue()
        {
            string[] trustedDomainCollection =
            {
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>()
            };
            ITrustedDomainHelper sut = CreateSut(trustedDomainCollection);

            bool result = sut.IsTrustedDomain(new Uri($"https://{trustedDomainCollection[_random.Next(0, trustedDomainCollection.Length - 1)]}/{_fixture.Create<string>()}"));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenUriIsNotTrustedDomain_ReturnsFalse()
        {
            string[] trustedDomainCollection =
            {
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>()
            };
            ITrustedDomainHelper sut = CreateSut(trustedDomainCollection);

            bool result = sut.IsTrustedDomain(new Uri($"https://{_fixture.Create<string>()}/{_fixture.Create<string>()}"));

            Assert.That(result, Is.False);
        }

        private ITrustedDomainHelper CreateSut(params string[] knownDomainCollection)
        {
            _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.TrustedDomainCollection) == 0)])
                .Returns(string.Join(";", knownDomainCollection ?? new[] {_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()}));

            return new Mvc.Helpers.Security.TrustedDomainHelper(_configurationMock.Object);
        }
    }
}