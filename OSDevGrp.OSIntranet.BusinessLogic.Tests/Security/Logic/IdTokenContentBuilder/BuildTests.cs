using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class BuildTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_AssertToClaimsWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object);

            sut.Build();

            userInfoMock.Verify(m => m.ToClaims(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollection()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueEqualToSubjectIdentifierFromConstructor()
        {
            string subjectIdentifier = _fixture.Create<string>();
            IIdTokenContentBuilder sut = CreateSut(subjectIdentifier: subjectIdentifier);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(subjectIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainSubjectIdentifierClaim_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainSubjectIdentifierClaim_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainSubjectIdentifierClaim_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesContainsSubjectIdentifierClaim_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueEqualToSubjectIdentifierFromConstructor()
        {
            string subjectIdentifier = _fixture.Create<string>();
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(subjectIdentifier: subjectIdentifier, userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(subjectIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueEqualToSubjectIdentifierFromConstructor()
        {
            string subjectIdentifier = _fixture.Create<string>();
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(subjectIdentifier: subjectIdentifier, userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(subjectIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsSubjectIdentifierClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueEqualToSubjectIdentifierFromConstructor()
        {
            string subjectIdentifier = _fixture.Create<string>();
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasSubjectIdentifierClaim: true, hasSubjectIdentifierClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(subjectIdentifier: subjectIdentifier, userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(subjectIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionContainingAllClaimsWithValueResolvedByUserInfo()
        {
            Claim[] additionalClaims = _fixture.CreateClaims(_random);
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(additionalClaims: additionalClaims);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(additionalClaims.All(additionalClaim => result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, additionalClaim.Type) == 0 && string.CompareOrdinal(claim.Value, additionalClaim.Value) == 0 && string.CompareOrdinal(claim.ValueType, additionalClaim.ValueType) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionNotContainingClaimsWithoutValueResolvedByUserInfo()
        {
            Claim[] additionalClaims =
            [
                _fixture.CreateClaim(hasValue: false),
                _fixture.CreateClaim(hasValue: false),
                _fixture.CreateClaim(hasValue: false)
            ];
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(additionalClaims: additionalClaims);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(additionalClaims.All(additionalClaim => result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, additionalClaim.Type) == 0) == null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeIsUtcTime_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeIsLocalTime_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.Now;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime.ToUniversalTime())));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainAuthenticationTimeClaim_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainAuthenticationTimeClaim_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainAuthenticationTimeClaim_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoDoesNotContainAuthenticationTimeClaim_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue:false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenClaimsResolvedByUserInfoContainsAuthenticationTimeClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasAuthenticationTimeClaim: true, hasAuthenticationTimeClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueEqualToValueFromCallToWithNonce()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string nonce = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithNonce(nonce).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.EqualTo(nonce));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoDoesNotContainNonceClaim_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoDoesNotContainNonceClaim_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoDoesNotContainNonceClaim_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoDoesNotContainNonceClaim_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueEqualToValueFromCallToWithNonce()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            string nonce = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithNonce(nonce).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.EqualTo(nonce));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithoutValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueEqualToValueFromCallToWithNonce()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            string nonce = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithNonce(nonce).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.EqualTo(nonce));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueNotEqualToNull()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithNonEmptyValue()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithValue_ReturnsNonEmptyClaimsCollectionContainingOneNonceClaimTypeWithValueEqualToValueFromCallToWithNonce()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            string nonce = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithNonce(nonce).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.EqualTo(nonce));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasNotBeenCalled_ReturnsNonEmptyClaimsCollectionWithoutNonceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasNotBeenCalledAndClaimsResolvedByUserInfoDoesNotContainNonceClaim_ReturnsNonEmptyClaimsCollectionWithoutNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasNotBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithoutValue_ReturnsNonEmptyClaimsCollectionWithoutNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: false);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasNotBeenCalledAndClaimsResolvedByUserInfoContainsNonceClaimWithValue_ReturnsNonEmptyClaimsCollectionWithoutNonceClaimType()
        {
            IEnumerable<Claim> claimsForUserInfo = CreateClaimsForUserInfo(hasNonceClaim: true, hasNonceClaimValue: true);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claimsForUserInfo).Object;
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferenceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthenticationContextClassReferenceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationContextClassReference(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferenceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthenticationContextClassReferenceClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationContextClassReference(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferenceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthenticationContextClassReferenceClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationContextClassReference(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferenceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthenticationContextClassReferenceClaimTypeWithValueEqualToValueFromCallToWithAuthenticationContextClassReference()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string authenticationContextClassReference = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithAuthenticationContextClassReference(authenticationContextClassReference).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType) == 0).Value, Is.EqualTo(authenticationContextClassReference));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferenceHasNotBeenCalled_ReturnsNonEmptyClaimsCollectionWithoutAuthenticationContextClassReferenceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasBeenCalledWithNonNullEmptyOrWhiteSpaceValues_ReturnsNonEmptyClaimsCollectionContainingAuthenticationMethodsReferencesClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationMethodsReferences(_fixture.CreateMany<string>(_random.Next(5, 10))).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasBeenCalledWithNonNullEmptyOrWhiteSpaceValues_ReturnsNonEmptyClaimsCollectionContainingAuthenticationMethodsReferencesClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationMethodsReferences(_fixture.CreateMany<string>(_random.Next(5, 10))).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasBeenCalledWithNonNullEmptyOrWhiteSpaceValues_ReturnsNonEmptyClaimsCollectionContainingAuthenticationMethodsReferencesClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationMethodsReferences(_fixture.CreateMany<string>(_random.Next(5, 10))).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasBeenCalledWithNonNullEmptyOrWhiteSpaceValues_ReturnsNonEmptyClaimsCollectionContainingAuthenticationMethodsReferencesClaimTypeWithValueEqualToJsonValueFromCallToWithAuthenticationMethodsReferences()
        {
            IEnumerable<string> authenticationMethodsReferences = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationMethodsReferences(authenticationMethodsReferences).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0).Value, Is.EqualTo(JsonSerializer.Serialize(authenticationMethodsReferences)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasBeenCalledWithOnlyNullEmptyAndWhiteSpaceValues_ReturnsNonEmptyClaimsCollectionWithoutAuthenticationMethodsReferencesClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthenticationMethodsReferences([null, string.Empty, " "]).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationMethodsReferencesHasNotBeenCalled_ReturnsNonEmptyClaimsCollectionWithoutAuthenticationMethodsReferencesClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthorizedPartyHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthorizedPartyClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthorizedParty(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthorizedPartyHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthorizedPartyClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthorizedParty(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthorizedPartyHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthorizedPartyClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithAuthorizedParty(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthorizedPartyHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingAuthorizedPartyClaimTypeWithValueEqualToValueFromCallToWithAuthorizedParty()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string authorizedParty = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithAuthorizedParty(authorizedParty).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType) == 0).Value, Is.EqualTo(authorizedParty));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthorizedPartyHasNotBeenCalled_ReturnsNonEmptyClaimsCollectionWithoutAuthorizedPartyClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType) == 0), Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledWithValueEqualToNull_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaim()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithCustomClaim(claimType, null).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, claimType) == 0 && string.CompareOrdinal(claim.Value, string.Empty) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledWithEmptyValue_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaim()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithCustomClaim(claimType, string.Empty).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, claimType) == 0 && string.CompareOrdinal(claim.Value, string.Empty) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledWithEqualToWhiteSpace_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaim()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithCustomClaim(claimType, " ").Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, claimType) == 0 && string.CompareOrdinal(claim.Value, string.Empty) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledWithNonEmptyValue_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaim()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            string value = _fixture.Create<string>();
            IEnumerable<Claim> result = sut.WithCustomClaim(claimType, value).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, claimType) == 0 && string.CompareOrdinal(claim.Value, value) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledMultipleTimesWithDifferentClaimTypes_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaims()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IDictionary<string, string> customClaims = new Dictionary<string, string>
            {
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()}
            };
            foreach (KeyValuePair<string, string> customClaim in customClaims)
            {
                sut = sut.WithCustomClaim(customClaim.Key, customClaim.Value);
            }
            IEnumerable<Claim> result = sut.Build();

            Assert.That(customClaims.All(customClaim => result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, customClaim.Key) == 0 && string.CompareOrdinal(claim.Value, customClaim.Value) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimHasBeenCalledMultipleTimesWithSameClaimType_ReturnsNonEmptyClaimsCollectionContainingMatchingCustomClaims()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IList<KeyValuePair<string, string>> customClaims = new List<KeyValuePair<string, string>>
            {
                new(claimType, _fixture.Create<string>()),
                new(claimType, _fixture.Create<string>()),
                new(claimType, _fixture.Create<string>()),
                new(claimType, _fixture.Create<string>()),
                new(claimType, _fixture.Create<string>())
            };
            foreach (KeyValuePair<string, string> customClaim in customClaims)
            {
                sut = sut.WithCustomClaim(customClaim.Key, customClaim.Value);
            }
            IEnumerable<Claim> result = sut.Build();

            Assert.That(customClaims.All(customClaim => result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, customClaim.Key) == 0 && string.CompareOrdinal(claim.Value, customClaim.Value) == 0) != null), Is.True);
        }

        private IIdTokenContentBuilder CreateSut(string subjectIdentifier = null, IUserInfo userInfo = null, DateTimeOffset? authenticationTime = null)
        {
            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(subjectIdentifier ?? _fixture.Create<string>(), userInfo ?? _fixture.BuildUserInfoMock().Object, authenticationTime ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
        }

        private IEnumerable<Claim> CreateClaimsForUserInfo(bool hasSubjectIdentifierClaim = true, bool hasSubjectIdentifierClaimValue = true, bool hasAuthenticationTimeClaim = true, bool hasAuthenticationTimeClaimValue = true, bool hasNonceClaim = true, bool hasNonceClaimValue = true, IEnumerable<Claim> additionalClaims = null)
        {
            List<Claim> claims = [];
            if (hasSubjectIdentifierClaim)
            {
                claims.Add(_fixture.CreateClaim(BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType, hasValue: hasSubjectIdentifierClaimValue));
            }
            if (hasAuthenticationTimeClaim)
            {
                claims.Add(_fixture.CreateClaim(BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType, hasValue: hasAuthenticationTimeClaimValue));
            }
            if (hasNonceClaim)
            {
                claims.Add(_fixture.CreateClaim(BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType, hasValue: hasNonceClaimValue));
            }

            claims.AddRange(additionalClaims ?? _fixture.CreateClaims(_random));

            return claims;
        }

        private static string CalculateExpectedValueForAuthenticationTimeClaimType(DateTimeOffset authenticationTime)
        {
            return authenticationTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        }
    }
}