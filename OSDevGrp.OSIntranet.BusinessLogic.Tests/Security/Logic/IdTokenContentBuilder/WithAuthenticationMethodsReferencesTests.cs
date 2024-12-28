using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithAuthenticationMethodsReferencesTests
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
        public void WithAuthenticationMethodsReferences_WhenAuthenticationMethodsReferencesIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthenticationMethodsReferences(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authenticationMethodsReferences"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationMethodsReferences_WhenAuthenticationMethodsReferencesIsEmptyCollection_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthenticationMethodsReferences(Array.Empty<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationMethodsReferences_WhenAuthenticationMethodsReferencesIsEmptyCollection_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthenticationMethodsReferences(Array.Empty<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationMethodsReferences_WhenAuthenticationMethodsReferencesIsNoneEmptyCollection_ReturnsNotNull()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthenticationMethodsReferences(_fixture.CreateMany<string>(_random.Next(5, 10)).ToArray());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthenticationMethodsReferences_WhenAuthenticationMethodsReferencesIsNoneEmptyCollection_ReturnsSameIdTokenContentBuilder()
        {
            IIdTokenContentBuilder sut = CreateSut();

            IIdTokenContentBuilder result = sut.WithAuthenticationMethodsReferences(_fixture.CreateMany<string>(_random.Next(5, 10)).ToArray());

            Assert.That(result, Is.SameAs(sut));
        }

        private IIdTokenContentBuilder CreateSut()
        {
            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(_fixture.Create<string>(), DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));
        }
    }
}