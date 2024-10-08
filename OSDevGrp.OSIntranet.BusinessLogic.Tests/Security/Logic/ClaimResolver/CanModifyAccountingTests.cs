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
    public class CanModifyAccountingTests
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
        public void CanModifyAccounting_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.CanModifyAccounting(_fixture.Create<int>());

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void CanModifyAccounting_WhenCalledAndPrincipalDoesNotHaveAccountingModifierClaim_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void CanModifyAccounting_WhenCalledAndPrincipalHasAccountingModifierClaimWhereValueIsEmpty_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingModifierClaimType, hasValue: false)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void CanModifyAccounting_WhenCalledAndPrincipalHasAccountingModifierClaimWhereValueIsWhiteSpace_ReturnsFalse()
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingModifierClaimType, value: " ")));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("XXX")]
        [TestCase("XXX,YYY")]
        [TestCase("XXX,YYY,ZZZ")]
        public void CanModifyAccounting_WhenCalledAndPrincipalHasAccountingModifierClaimWhereValueIsNoneIntegerCollection_ReturnsFalse(string claimValue)
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(ClaimHelper.AccountingModifierClaimType, value: claimValue)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(_fixture.Create<int>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public void CanModifyAccounting_WhenCalledAndPrincipalHasAccountingModifierClaimWhereValueIsIntegerCollectionWithoutWildcard_ReturnsExpectedResult(int accountingNumber, bool expectedResult)
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateAccountingModifierClaim(false, 1, 3, 5)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(accountingNumber);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        public void CanModifyAccounting_WhenCalledAndPrincipalHasAccountingModifierClaimWhereValueIsIntegerCollectionWithWildcard_ReturnsExpectedResult(int accountingNumber, bool expectedResult)
        {
            IPrincipal principal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(ClaimHelper.CreateAccountingModifierClaim(true, 1, 3, 5)));
            IClaimResolver sut = CreateSut(principal);

            bool result = sut.CanModifyAccounting(accountingNumber);

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