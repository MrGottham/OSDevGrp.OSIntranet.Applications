using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
    [TestFixture]
    public class GetCountryCode
    {
        #region Private variables

        private Mock<IPrincipalResolver> _principalResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _principalResolverMock = new Mock<IPrincipalResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void GetCountryCode_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.GetCountryCode();

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetCountryCode_WhenCalledAndPrincipalDoesNotHaveCountryCodeClaim_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>())});
            IClaimResolver sut = CreateSut(principal);

            string result = sut.GetCountryCode();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetCountryCode_WhenCalledAndPrincipalHasCountryCodeClaim_ReturnsCountryCode()
        {
            string countryCode = _fixture.Create<string>();
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateCountryCodeClaim(countryCode)});
            IClaimResolver sut = CreateSut(principal);

            string result = sut.GetCountryCode();

            Assert.That(result, Is.EqualTo(countryCode));
        }

        private IClaimResolver CreateSut(IPrincipal principal = null)
        {
            _principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(principal ?? CreateClaimsPrincipal());

            return new BusinessLogic.Security.Logic.ClaimResolver(_principalResolverMock.Object);
        }

        private IPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claimCollection = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claimCollection ?? new Claim[] { }));
        }
    }
}
