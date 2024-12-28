using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
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
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingSubjectIdentifierClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingSubjectIdentifierClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingSubjectIdentifierClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingSubjectIdentifierClaimTypeWithValueEqualToSubjectIdentifierFromConstructor()
        {
            string subjectIdentifier = _fixture.Create<string>();
            IIdTokenContentBuilder sut = CreateSut(subjectIdentifier: subjectIdentifier);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType) == 0).Value, Is.EqualTo(subjectIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingAuthenticationTimeClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingAuthenticationTimeClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_Called_ReturnsNonEmptyClaimsCollectionContainingAuthenticationTimeClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeIsUtcTime_ReturnsNonEmptyClaimsCollectionContainingAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenAuthenticationTimeIsLocalTime_ReturnsNonEmptyClaimsCollectionContainingAuthenticationTimeClaimTypeWithValueEqualToUnixTimeSeconds()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.Now;
            IIdTokenContentBuilder sut = CreateSut(authenticationTime: authenticationTime);

            IEnumerable<Claim> result = sut.Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType) == 0).Value, Is.EqualTo(CalculateExpectedValueForAuthenticationTimeClaimType(authenticationTime.ToUniversalTime())));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingNonceClaimType()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.SingleOrDefault(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingNonceClaimTypeWithValueNotEqualToNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingNonceClaimTypeWithNonEmptyValue()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IEnumerable<Claim> result = sut.WithNonce(_fixture.Create<string>()).Build();

            Assert.That(result.Single(claim => string.CompareOrdinal(claim.Type, BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType) == 0).Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithNonceHasBeenCalled_ReturnsNonEmptyClaimsCollectionContainingNonceClaimTypeWithValueEqualToValueFromCallToWithNonce()
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

        private IIdTokenContentBuilder CreateSut(string subjectIdentifier = null, DateTimeOffset? authenticationTime = null)
        {
            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(subjectIdentifier ?? _fixture.Create<string>(), authenticationTime ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
        }

        private static string CalculateExpectedValueForAuthenticationTimeClaimType(DateTimeOffset authenticationTime)
        {
            return authenticationTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        }
    }
}