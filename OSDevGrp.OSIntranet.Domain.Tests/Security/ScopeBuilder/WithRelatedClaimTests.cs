using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ScopeBuilder
{
    [TestFixture]
    public class WithRelatedClaimTests : ScopeBuilderTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithRelatedClaim_WhenClaimTypeIsNull_ThrowsArgumentNullException()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() =>  sut.WithRelatedClaim(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRelatedClaim_WhenClaimTypeIsEmpty_ThrowsArgumentNullException()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRelatedClaim(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRelatedClaim_WhenClaimTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRelatedClaim(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimType"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRelatedClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScopeBuilder result = sut.WithRelatedClaim(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRelatedClaim_WhenClaimTypeIsNotNullEmptyOrWhiteSpace_ReturnsSameScopeBuilder()
        {
            IScopeBuilder sut = CreateSut(_fixture);

            IScopeBuilder result = sut.WithRelatedClaim(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }
    }
}