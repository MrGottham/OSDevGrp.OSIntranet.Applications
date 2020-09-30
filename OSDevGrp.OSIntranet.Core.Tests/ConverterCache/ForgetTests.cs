using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.ConverterCache
{
    [TestFixture]
    public class ForgetTests
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
        public void Forget_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Forget<IConverterCache>(null));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void Forget_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Forget<IConverterCache>(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void Forget_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Forget<IConverterCache>(" "));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void Forget_WhenKeyDoesNotExistInMemory_ThrowsNoException()
        {
            IConverterCache sut = CreateSut();

            sut.Forget<IConverterCache>(_fixture.Create<string>());
        }

        [Test]
        [Category("UnitTest")]
        public void Forget_WhenKeyExistsInMemory_AssertObjectHasBeenForgotten()
        {
            IConverterCache sut = CreateSut();

            string key = _fixture.Create<string>();
            sut.Remember(sut, m => key);

            sut.Forget<IConverterCache>(key);

            IConverterCache result = sut.FromMemory<IConverterCache>(key);

            Assert.That(result, Is.Null);
        }

        private IConverterCache CreateSut()
        {
            return new Core.ConverterCache();
        }
    }
}