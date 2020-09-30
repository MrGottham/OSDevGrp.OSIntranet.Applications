using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core.Tests.ConverterCache
{
    [TestFixture]
    public class SyncRootTests
    {
        [Test]
        [Category("UnitTest")]
        public void SyncRoot_WhenCalled_ReturnsNotNull()
        {
            IConverterCache sut = CreateSut();

            object result = sut.SyncRoot;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SyncRoot_WhenCalledMultipleTimes_ReturnsSameSyncRoot()
        {
            IConverterCache sut = CreateSut();

            object result = sut.SyncRoot;

            Assert.That(result, Is.SameAs(sut.SyncRoot));
        }

        private IConverterCache CreateSut()
        {
            return new Core.ConverterCache();
        }
    }
}