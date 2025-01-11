using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithCustomClaimsFilteredByClaimTypeTests : IdTokenContentBuilderTestBase
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
        public void WithCustomClaimsFilteredByClaimType_WhenClaimTypeIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByClaimType(null, _fixture.CreateClaims(_random)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByClaimType_WhenClaimTypeIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByClaimType(string.Empty, _fixture.CreateClaims(_random)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByClaimType_WhenClaimTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByClaimType(" ", _fixture.CreateClaims(_random)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByClaimType_WhenCustomClaimsIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByClaimType(_fixture.Create<string>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("customClaims"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByClaimType_WhenCalled_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByClaimType(_fixture.Create<string>(), _fixture.CreateClaims(_random));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByClaimType_WhenCalled_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByClaimType(_fixture.Create<string>(), _fixture.CreateClaims(_random));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}