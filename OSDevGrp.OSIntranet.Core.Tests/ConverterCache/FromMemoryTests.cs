using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.ConverterCache
{
    [TestFixture]
    public class FromMemoryTests
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
        public void FromMemory_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromMemory<IConverterCache>(null));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromMemory_WhenKeyIsEmpty_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromMemory<IConverterCache>(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromMemory_WhenKeyIsWhiteSpace_ThrowsArgumentNullException()
        {
            IConverterCache sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromMemory<IConverterCache>(" "));

            Assert.That(result.ParamName, Is.EqualTo("key"));
        }

        [Test]
        [Category("UnitTest")]
        public void FromMemory_WhenKeyDoesNotExistInMemory_ReturnsNull()
        {
            IConverterCache sut = CreateSut();

            IConverterCache result = sut.FromMemory<IConverterCache>(_fixture.Create<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromMemory_WhenKeyExistsInMemory_ReturnsNotNull()
        {
            IConverterCache sut = CreateSut();

            string key = _fixture.Create<string>();
            sut.Remember(sut, m => key);

            IConverterCache result = sut.FromMemory<IConverterCache>(key);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void FromMemory_WhenKeyExistsInMemory_ReturnsObjectFromCache()
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