using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class AddClaimsTests : ClientSecretIdentityBuilderTestBase
    {
        #region Private variables

        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            _random = new Random(Fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddClaims_WhenClaimsIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddClaims(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdentifier_WhenCalled_ReturnsClientSecretIdentityBuilder()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentityBuilder result = sut.AddClaims(Fixture.CreateClaims(_random));

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}