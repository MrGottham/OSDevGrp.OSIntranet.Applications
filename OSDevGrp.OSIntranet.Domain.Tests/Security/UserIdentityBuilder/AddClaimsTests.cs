using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentityBuilder
{
    [TestFixture]
    public class AddClaimsTests : UserIdentityBuilderTestBase
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
            IUserIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddClaims(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddClaims_WhenCalled_ReturnsUserIdentityBuilder()
        {
            IUserIdentityBuilder sut = CreateSut();

            IUserIdentityBuilder result = sut.AddClaims(Fixture.CreateClaims(_random));

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}