using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.ConverterCache
{
    [TestFixture]
    public class RememberTests
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
        public void Remember_WhenObjectIsNull_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Remember<IConverterCache>(null, m => _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("obj"));
        }

        [Test]
        [Category("UnitTest")]
        public void Remember_WhenKeyGeneratorIsNull_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Remember(sut, null));

            Assert.That(result.ParamName, Is.EqualTo("keyGenerator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Remember_WhenCalled_AssertKeyGeneratorWasCalled()
        {
            IConverterCache sut = CreateSut();

            bool keyGeneratorWasCalled = false;
            sut.Remember(sut, m =>
            {
                keyGeneratorWasCalled = true;
                return _fixture.Create<string>();
            });

            Assert.That(keyGeneratorWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Remember_WhenCalled_AssertObjectWasRememberedByCache()
        {
            IConverterCache sut = CreateSut();

            string key = _fixture.Create<string>();
            sut.Remember(sut, m => key);

            IConverterCache result = sut.FromMemory<IConverterCache>(key);

            Assert.That(result, Is.SameAs(sut));
        }

        private IConverterCache CreateSut()
        {
            return new Core.ConverterCache();
        }
    }
}