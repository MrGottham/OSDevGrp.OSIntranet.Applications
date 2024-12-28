using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithAuthorizedPartyTests
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
        public void WithAuthorizedParty_WhenAuthorizedPartyIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizedParty(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizedParty"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizedParty_WhenAuthorizedPartyIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizedParty(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizedParty"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizedParty_WhenAuthorizedPartyIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizedParty(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authorizedParty"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizedParty_WhenAuthorizedPartyIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthorizedParty(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizedParty_WhenAuthorizedPartyIsNotNullEmptyOrWhiteSpace_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthorizedParty(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IIdTokenContentBuilder CreateSut()
        {
            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(_fixture.Create<string>(), DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
        }
    }
}