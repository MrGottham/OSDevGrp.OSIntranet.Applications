using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserInfoFactory
{
    [TestFixture]
    public class FromPrincipalTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenClaimsPrincipalIsNull_ThrowsArgumentNullException()
        {
            IUserInfoFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromPrincipal(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimsPrincipal"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenCalled_ReturnsNotNull()
        {
            IUserInfoFactory sut = CreateSut();

            IUserInfo result = sut.FromPrincipal(CreateClaimsPrincipal());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenCalled_ReturnsUserInfo()
        {
            IUserInfoFactory sut = CreateSut();

            IUserInfo result = sut.FromPrincipal(CreateClaimsPrincipal());

            Assert.That(result, Is.TypeOf<Domain.Security.UserInfo>());
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenCalled_ReturnsUserInfoWhereSubjectIsNotNull()
        {
            IUserInfoFactory sut = CreateSut();

            IUserInfo result = sut.FromPrincipal(CreateClaimsPrincipal());

            Assert.That(result.Subject, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenCalled_ReturnsUserInfoWhereSubjectIsNotEmpty()
        {
            IUserInfoFactory sut = CreateSut();

            IUserInfo result = sut.FromPrincipal(CreateClaimsPrincipal());

            Assert.That(result.Subject, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void FromPrincipal_WhenCalled_ReturnsUserInfoWhereSubjectIsEqualToNameIdentifier()
        {
            IUserInfoFactory sut = CreateSut();

            string nameIdentifier = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(nameIdentifier: nameIdentifier);
            IUserInfo result = sut.FromPrincipal(claimsPrincipal);

            Assert.That(result.Subject, Is.EqualTo(nameIdentifier));
        }

        private IUserInfoFactory CreateSut()
        {
            return new Domain.Security.UserInfoFactory();
        }

        private ClaimsPrincipal CreateClaimsPrincipal(string nameIdentifier = null)
        {
            Claim[] claims =
            [
                _fixture.CreateClaim(type: ClaimTypes.NameIdentifier, hasValue: true, value: nameIdentifier)
            ];

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}