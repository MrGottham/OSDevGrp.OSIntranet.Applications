using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.TrustedDomainResolver
{
    [TestFixture]
    public class IsTrustedDomainTests
    {
        #region Private variables

        private Mock<IOptions<TrustedDomainOptions>> _trustedDomainOptionsMock;
        private Fixture _fixture;
        private Random _random;

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
        public void IsTrustedDomain_WhenUriIsNull_ThrowsArgumentNullException()
        {
            ITrustedDomainResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsTrustedDomain(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("uri"));
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenCalled_AssertValueWasCalledOnOptionsForTrustedDomainOptions()
        {
            ITrustedDomainResolver sut = CreateSut();

            sut.IsTrustedDomain(_fixture.CreateEndpoint());

            _trustedDomainOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenUriIsTrustedDomain_ReturnTrue()
        {
            string[] trustedDomainCollection = CreateTrustedDomainCollection();
            ITrustedDomainResolver sut = CreateSut(trustedDomainCollection);

            bool result = sut.IsTrustedDomain(_fixture.CreateEndpoint(domainName: trustedDomainCollection[_random.Next(0, trustedDomainCollection.Length - 1)]));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsTrustedDomain_WhenUriIsNotTrustedDomain_ReturnFalse()
        {
            string[] trustedDomainCollection = CreateTrustedDomainCollection();
            ITrustedDomainResolver sut = CreateSut(trustedDomainCollection);

            bool result = sut.IsTrustedDomain(_fixture.CreateEndpoint(domainName: _fixture.CreateDomainName()));

            Assert.That(result, Is.False);
        }

        private ITrustedDomainResolver CreateSut(string[] trustedDomainCollection = null)
        {
            _trustedDomainOptionsMock.Setup(m => m.Value)
                .Returns(CreateTrustedDomainOptions(trustedDomainCollection));

            return new Core.Resolvers.TrustedDomainResolver(_trustedDomainOptionsMock.Object);
        }

        private TrustedDomainOptions CreateTrustedDomainOptions(string[] trustedDomainCollection = null)
        {
            trustedDomainCollection ??= CreateTrustedDomainCollection();

            return new TrustedDomainOptions
            {
                TrustedDomainCollection = string.Join(';', trustedDomainCollection.Select(m => m))
            };
        }

        private string[] CreateTrustedDomainCollection()
        {
            return
            [
                _fixture.CreateDomainName(),
                _fixture.CreateDomainName(),
                _fixture.CreateDomainName(),
                _fixture.CreateDomainName(),
                _fixture.CreateDomainName()
            ];
        }
    }
}