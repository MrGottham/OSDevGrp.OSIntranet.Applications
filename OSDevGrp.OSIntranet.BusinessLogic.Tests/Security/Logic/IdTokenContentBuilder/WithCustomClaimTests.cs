using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithCustomClaimTests
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
        public void WithCustomClaim_WhenClaimTypeIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaim(null, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaim(string.Empty, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaim(" ", _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.SubjectIdentifierClaimType)]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationTimeClaimType)]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.NonceClaimType)]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationContextClassReferenceClaimType)]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthenticationMethodsReferencesClaimType)]
        [TestCase(BusinessLogic.Security.Logic.IdTokenContentBuilder.AuthorizedPartyClaimType)]
        public void WithCustomClaim_WhenClaimTypeIsNonSupportedValue_ThrowsArgumentException(string claimType)
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentException result = Assert.Throws<ArgumentException>(() => sut.WithCustomClaim(claimType, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Does.StartWith($"Cannot add a custom claim with the type: {claimType}"));
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsNull_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsNull_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), null);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsEmpty_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsEmpty_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsWhiteSpace_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceButValueIsWhiteSpace_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), " ");

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceAndValueIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpaceAndValueIsNotNullEmptyOrWhiteSpace_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaim(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IIdTokenContentBuilder CreateSut()
        {
            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(_fixture.Create<string>(), _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
        }
    }
}