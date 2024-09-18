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
    public class GetNumberOfNewsToCollectTests
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
        public void GetNumberOfNewsToCollect_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.GetNumberOfNewsToCollect();

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNumberOfNewsToCollect_WhenCalledAndPrincipalDoesNotHaveCollectNewsClaim_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random));
            IClaimResolver sut = CreateSut(principal);

            int? result = sut.GetNumberOfNewsToCollect();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNumberOfNewsToCollect_WhenCalledAndPrincipalHasCollectNewsClaimWithoutClaimValue_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateCollectNewsClaim()));
            IClaimResolver sut = CreateSut(principal);

            int? result = sut.GetNumberOfNewsToCollect();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNumberOfNewsToCollect_WhenCalledAndPrincipalHasCollectNewsClaimWithNonIntegerClaimValue_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateClaim(ClaimHelper.CollectNewsClaimType, _fixture.Create<string>())));
            IClaimResolver sut = CreateSut(principal);

            int? result = sut.GetNumberOfNewsToCollect();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNumberOfNewsToCollect_WhenCalledAndPrincipalHasCollectNewsClaimWithIntegerClaimValue_ReturnsNotNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateCollectNewsClaim(_fixture.Create<int>())));
            IClaimResolver sut = CreateSut(principal);

            int? result = sut.GetNumberOfNewsToCollect();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetNumberOfNewsToCollect_WhenCalledAndPrincipalHasCollectNewsClaimWithIntegerClaimValue_ReturnsNumberOfNewsToCollect()
        {
            int numberOfNewsToCollect = _fixture.Create<int>();
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateCollectNewsClaim(numberOfNewsToCollect)));
            IClaimResolver sut = CreateSut(principal);

            int? result = sut.GetNumberOfNewsToCollect();

            Assert.That(result, Is.EqualTo(numberOfNewsToCollect));
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