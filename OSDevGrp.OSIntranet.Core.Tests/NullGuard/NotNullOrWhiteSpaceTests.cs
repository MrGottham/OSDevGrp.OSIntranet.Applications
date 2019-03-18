using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.NullGuard
{
    [TestFixture]
    public class NotNullOrWhiteSpaceTests
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
        public void NotNullOrWhiteSpace_WhenArgumentNameIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenArgumentNameIsEmpty_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenArgumentNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenValueIsNull_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(null, argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenValueIsEmpty_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(string.Empty, argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            INullGuard sut = CreateSut();

            string argumentName = _fixture.Create<string>();
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.NotNullOrWhiteSpace(" ", argumentName));

            Assert.That(result.ParamName, Is.EqualTo(argumentName));
        }

        [Test]
        [Category("UnitTest")]
        public void NotNullOrWhiteSpace_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsNullGuard()
        {
            INullGuard sut = CreateSut();

            INullGuard result = sut.NotNullOrWhiteSpace(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(sut, Is.EqualTo(result));
        }

        private INullGuard CreateSut()
        {
            return new Core.NullGuard();
        }
    }
}