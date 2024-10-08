﻿using AutoFixture;
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
    public class CanAccessAccountingTests
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
        public void CanAccessAccounting_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.CanAccessAccounting(_fixture.Create<int>());

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void CanAccessAccounting_WhenCalledAndPrincipalDoesNotHaveAccountingViewerClaim_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void CanAccessAccounting_WhenCalledAndPrincipalHasAccountingViewerClaimWhereValueIsEmpty_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingViewerClaimType, hasValue: false)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void CanAccessAccounting_WhenCalledAndPrincipalHasAccountingViewerClaimWhereValueIsWhiteSpace_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingViewerClaimType, value: " ")));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX")]
        [TestCase("XXX,YYY")]
        [TestCase("XXX,YYY,ZZZ")]
        public void CanAccessAccounting_WhenCalledAndPrincipalHasAccountingViewerClaimWhereValueIsNoneIntegerCollection_ReturnsFalse(string claimValue)
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingViewerClaimType, value: claimValue)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public void CanAccessAccounting_WhenCalledAndPrincipalHasAccountingViewerClaimWhereValueIsIntegerCollectionWithoutWildcard_ReturnsExpectedResult(int accountingNumber, bool expectedResult)
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateAccountingViewerClaim(false, 1, 3, 5)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(accountingNumber);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        public void CanAccessAccounting_WhenCalledAndPrincipalHasAccountingViewerClaimWhereValueIsIntegerCollectionWithWildcard_ReturnsExpectedResult(int accountingNumber, bool expectedResult)
        {
            
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateAccountingViewerClaim(true, 1, 3, 5)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanAccessAccounting(accountingNumber);

            Assert.That(result, Is.EqualTo(expectedResult));
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