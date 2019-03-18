using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.NullGuard
{
    [TestFixture]
    public class NotNullTests
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
        public void NotNull_WhenArgumentNameIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(_fixture.Create<object>(), null));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNull_WhenArgumentNameIsEmpty_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(_fixture.Create<object>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNull_WhenArgumentNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(_fixture.Create<object>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNull_WhenValueIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNull(null, argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNull_WhenValueIsNotNull_ReturnsNullGuard()
        {
            INullGuard sut = CreateSut();

            INullGuard result = sut.NotNull(_fixture.Create<object>(), _fixture.Create<string>());

            Assert.That(sut, Is.EqualTo(result));
        }
        
        private INullGuard CreateSut()
        {
            return new Core.NullGuard();
        }
    }
}