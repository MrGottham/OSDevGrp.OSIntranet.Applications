using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
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
    public class BuildTests : IdTokenContentBuilderTestBase
    {
        #region Private variables

        private Mock<IClaimsSelector> _claimSelectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimSelectorMock = new Mock<IClaimsSelector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        protected override Mock<IClaimsSelector> ClaimsSelectorMock => _claimSelectorMock;

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_AssertSelectWasNotCalledOnClaimsSelector()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            Assert.Throws<IntranetBusinessException>(() => sut.Build());

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_ThrowsIntranetBusinessException()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToGenerateIdTokenForAuthenticatedUser()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeNotInScopesGivenByConstructor_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.Build());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_AssertSelectWasCalledOnClaimsSelectorWithOpenIdScopeAsOnlyScopeAndClaimsIsEmpty()
        {
            string nameIdentifier = _fixture.Create<string>();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(nameIdentifier: nameIdentifier, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.OpenIdScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.Any() == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimType()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionContainingOneSubjectIdentifierClaimTypeTypeWithValueEqualToNameIdentifierGivenByConstructor()
        {
            string nameIdentifier = _fixture.Create<string>();
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(nameIdentifier: nameIdentifier, scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(nameIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenOpenIdScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithSelectedClaimsForOpenIdScope()
        {
            IEnumerable<Claim> selectedClaims = _fixture.CreateClaims(Random);
            IScope openIdScope = CreateOpenIdScope(selectedClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScope, CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withOpenIdScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(selectedClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, selectedClaim.Value) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_AssertFullNameWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.FullName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoHasFullName_AssertSelectWasCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingMatchingClaimForName()
        {
            string fullName = _fixture.Create<string>();
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasFullName: true, fullName: fullName).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Name) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, fullName) == 0) != null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoDoesNotHaveFullName_AssertSelectWasNotCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingClaimForName()
        {
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasFullName: false).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Name) == 0) != null)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_AssertGivenNameWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.GivenName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoHasGivenName_AssertSelectWasCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingMatchingClaimForGivenName()
        {
            string givenName = _fixture.Create<string>();
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasGivenName: true, givenName: givenName).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.GivenName) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, givenName) == 0) != null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoDoesNotHaveGivenName_AssertSelectWasNotCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingClaimForGivenName()
        {
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasGivenName: false).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.GivenName) == 0) != null)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_AssertSurnameWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.Surname, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoHasSurname_AssertSelectWasCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingMatchingClaimForSurname()
        {
            string surname = _fixture.Create<string>();
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasSurname: true, surname: surname).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Surname) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, surname) == 0) != null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoDoesNotHaveSurname_AssertSelectWasNotCalledOnClaimsSelectorWithProfileScopeAsOnlyScopeAndClaimsContainingClaimForSurname()
        {
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasSurname: false).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Surname) == 0) != null)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_ReturnsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithSelectedClaimsForProfileScope()
        {
            IEnumerable<Claim> selectedClaims = _fixture.CreateClaims(Random);
            IScope profileScope = CreateProfileScope(selectedClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), profileScope, CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(selectedClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, selectedClaim.Value) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeNotInScopesGivenByConstructor_AssertFullNameWasNotCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: false);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.FullName, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeNotInScopesGivenByConstructor_AssertGivenNameWasNotCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: false);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.GivenName, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeNotInScopesGivenByConstructor_AssertSurnameWasNotCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: false);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.Surname, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeNotInScopesGivenByConstructor_AssertSelectWasNotCalledOnClaimsSelectorWithProfileScopeAsOnlyScope()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeNotInScopesGivenByConstructor_ReturnsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeInScopesGivenByConstructor_AssertEmailWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.Email, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoHasEmail_AssertSelectWasCalledOnClaimsSelectorWithEmailScopeAsOnlyScopeAndClaimsContainingMatchingClaimForEmail()
        {
            string email = _fixture.Create<string>();
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasEmail: true, email: email).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.EmailScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Email) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, email) == 0) != null)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenProfileScopeInScopesGivenByConstructorAndUserInfoDoesNotHaveEmail_AssertSelectWasNotCalledOnClaimsSelectorWithEmailScopeAsOnlyScopeAndClaimsContainingClaimForEmail()
        {
            IUserInfo userInfo = _fixture.BuildUserInfoMock(hasEmail: false).Object;
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfo, supportedScopes: supportedScopes, scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.Is<IReadOnlyDictionary<string, IScope>>(value => value != null && value == supportedScopes),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.ProfileScope) == 0)),
                    It.Is<IEnumerable<Claim>>(value => value != null && value.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, ClaimTypes.Email) == 0) != null)),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeInScopesGivenByConstructor_ReturnsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: true);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithSelectedClaimsForEmailScope()
        {
            IEnumerable<Claim> selectedClaims = _fixture.CreateClaims(Random);
            IScope emailScope = CreateEmailScope(selectedClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), emailScope, CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(selectedClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, selectedClaim.Value) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeNotInScopesGivenByConstructor_AssertEmailWasNotCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: false);
            IIdTokenContentBuilder sut = CreateSut(userInfo: userInfoMock.Object, scopes: scopes);

            sut.Build();

            userInfoMock.Verify(m => m.Email, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeNotInScopesGivenByConstructor_AssertSelectWasNotCalledOnClaimsSelectorWithEmailScopeAsOnlyScope()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            sut.Build();

            ClaimsSelectorMock.Verify(m => m.Select(
                    It.IsAny<IReadOnlyDictionary<string, IScope>>(),
                    It.Is<IEnumerable<string>>(value => value != null && value.All(scope => string.IsNullOrWhiteSpace(scope) == false && string.CompareOrdinal(scope, ScopeHelper.EmailScope) == 0)),
                    It.IsAny<IEnumerable<Claim>>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenEmailScopeNotInScopesGivenByConstructor_ReturnsNotNull()
        {
            IReadOnlyCollection<string> scopes = CreateScopes(withEmailScope: false);
            IIdTokenContentBuilder sut = CreateSut(scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeGivenByConstructorIsUtcTime_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeGivenByConstructorIsLocalTime_ReturnsNonEmptyClaimsCollectionContainingOneAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.Now;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime.ToUniversalTime())));
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
        public void Build_WhenWithNonceHasNotBeenCalled_ReturnsNonEmptyClaimsCollectionWithoutNonceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

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
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInSupportedScopesGivenByConstructor_AssertFilterWasNotCalledOnScope(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random)).Build();

            scopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInSupportedScopesGivenByConstructor_ReturnsNotNull(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IScope scope = CreateWebApiScope();
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInSupportedScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IScope scope = CreateWebApiScope();
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInSupportedScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithoutFilteredClaimsFromScope(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> filteredClaims = _fixture.CreateClaims(_random);
            IScope scope = CreateWebApiScope(filteredClaims);
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(filteredClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0) == null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInScopesGivenByConstructor_AssertFilterWasNotCalledOnScope()
        {
            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random)).Build();

            scopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInScopesGivenByConstructor_ReturnsNotNull()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledButScopeNotInScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithoutFilteredClaimsFromScope()
        {
            IEnumerable<Claim> filteredClaims = _fixture.CreateClaims(_random);
            IScope scope = CreateWebApiScope(filteredClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(filteredClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0) == null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledAndScopeInBothSupportedScopesAndScopesGivenByConstructor_AssertFilterWasCalledOnScope()
        {
            IEnumerable<Claim> filteredClaims = _fixture.CreateClaims(_random);
            Mock<IScope> scopeMock = CreateWebApiScopeMock(filteredClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> customClaims = _fixture.CreateClaims(_random);
            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, customClaims).Build();

            scopeMock.Verify(m => m.Filter(It.Is<IEnumerable<Claim>>(value => value != null && value == customClaims)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledAndScopeInBothSupportedScopesAndScopesGivenByConstructor_ReturnsNotNull()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledAndScopeInBothSupportedScopesAndScopesGivenByConstructor_ReturnsNonEmptyClaimsCollection()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasBeenCalledAndScopeInBothSupportedScopesAndScopesGivenByConstructor_ReturnsNonEmptyClaimsCollectionWithFilteredClaimsFromScope()
        {
            IEnumerable<Claim> filteredClaims = _fixture.CreateClaims(_random);
            IScope scope = CreateWebApiScope(filteredClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random)).Build();

            Assert.That(filteredClaims.All(selectedClaim => result.SingleOrDefault(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, selectedClaim.Type) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, selectedClaim.Value) == 0) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasNotBeenCalled_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithCustomClaimsFilteredByScopeHasNotBeenCalled_ReturnsNonEmptyClaimsCollection()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasBeenCalled_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> customClaims = new List<Claim>(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(type: claimType)));
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByClaimType(claimType, customClaims).Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasBeenCalled_ReturnsNonEmptyClaimsCollection()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> customClaims = new List<Claim>(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(type: claimType)));
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByClaimType(claimType, customClaims).Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasBeenCalledWhereCustomClaimsContainingClaimTypeOnce_ReturnsNonEmptyClaimsCollectionWithFilteredCustomClaims()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            string claimValue = _fixture.Create<string>();
            IEnumerable<Claim> customClaims = new List<Claim>(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(type: claimType, value: claimValue)));
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByClaimType(claimType, customClaims).Build();

            Assert.That(result.SingleOrDefault(claim => string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, claimType) == 0 && string.IsNullOrWhiteSpace(claim.Value) == false && string.CompareOrdinal(claim.Value, claimValue) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasBeenCalledWhereCustomClaimsContainingClaimTypeMultipleTimes_ReturnsNonEmptyClaimsCollectionWithFilteredCustomClaims()
        {
            IIdTokenContentBuilder sut = CreateSut();

            string claimType = _fixture.Create<string>();
            IEnumerable<Claim> customClaims = new List<Claim>(_fixture.CreateClaims(_random).Concat(_fixture.CreateClaim(type: claimType), _fixture.CreateClaim(type: claimType), _fixture.CreateClaim(type: claimType)));
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByClaimType(claimType, customClaims).Build();

            Assert.That(result.Count(claim => string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, claimType) == 0), Is.EqualTo(3));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasBeenCalledWhereCustomClaimsNotContainingClaimType_ReturnsNonEmptyClaimsCollectionWithFilteredCustomClaims()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> customClaims = new List<Claim>(_fixture.CreateClaims(_random));
            IEnumerable<Claim> result = sut.WithCustomClaimsFilteredByClaimType(_fixture.Create<string>(), customClaims).Build();

            Assert.That(customClaims.All(customClaim => result.Any(claim => string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, customClaim.Type) == 0) == false), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasNotBeenCalled_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WithCustomClaimsFilteredByClaimTypeHasNotBeenCalled_ReturnsNonEmptyClaimsCollection()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimTypes.NameIdentifier)]
        public void Build_WhenCalled_ReturnsNonEmptyClaimsCollectionNotContainingNoneSupportedClaimTypes(string nonSupportedClaimType)
        {
            IEnumerable<Claim> selectedClaims =
            [
                _fixture.CreateClaim(nonSupportedClaimType)
            ];
            IScope profileScope = CreateProfileScope(selectedClaims);
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), profileScope, CreateEmailScope(), CreateWebApiScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withProfileScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.FirstOrDefault(claim => string.IsNullOrWhiteSpace(claim.Type) == false && string.CompareOrdinal(claim.Type, nonSupportedClaimType) == 0), Is.Null);
        }

        private static string CalculateExpectedValueForAuthenticationTimeClaimType(DateTimeOffset authenticationTime)
        {
            return authenticationTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        }
    }
}