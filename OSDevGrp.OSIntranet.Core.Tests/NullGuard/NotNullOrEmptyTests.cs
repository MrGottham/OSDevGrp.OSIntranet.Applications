using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.NullGuard
{
    [TestFixture]
    public class NotNullOrEmptyTests
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
        public void NotNullOrEmpty_WhenArgumentNameIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenArgumentNameIsEmpty_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenArgumentNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenValueIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(null, argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenValueIsEmpty_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrEmpty(string.Empty, argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenValueIsWhiteSpace_ReturnsNullGuard()
        {
            INullGuard sut = CreateSut();

            INullGuard result = sut.NotNullOrEmpty(" ", _fixture.Create<string>());

            Assert.That(sut, Is.EqualTo(result));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrEmpty_WhenValueIsNotNullOrEmpty_ReturnsNullGuard()
        {
            INullGuard sut = CreateSut();

            INullGuard result = sut.NotNullOrEmpty(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(sut, Is.EqualTo(result));
        }

        private INullGuard CreateSut()
        {
            return new Core.NullGuard();
        }
    }
}