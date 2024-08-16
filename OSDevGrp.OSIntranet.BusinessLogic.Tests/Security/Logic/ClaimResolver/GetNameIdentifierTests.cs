using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
    [TestFixture]
    public class GetNameIdentifierTests
    {
        #region Private variables

        private Mock<IPrincipalResolver> _principalResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _principalResolverMock = new Mock<IPrincipalResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetNameIdentifier_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.GetNameIdentifier();

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNameIdentifier_WhenCalledAndPrincipalDoesNotHaveNameIdentifierClaim_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random));
            IClaimResolver sut = CreateSut(principal);

            string result = sut.GetNameIdentifier();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNameIdentifier_WhenCalledAndPrincipalHasNameIdentifierClaim_ReturnsNameIdentifier()
        {
            string nameIdentifier = _fixture.Create<string>();
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateNameIdentifierClaim(nameIdentifier)));
            IClaimResolver sut = CreateSut(principal);

            string result = sut.GetNameIdentifier();

            Assert.That(result, Is.EqualTo(nameIdentifier));
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