using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class PushKeyValueEntryAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void PushKeyValueEntryAsync_WhenKeyIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PushKeyValueEntryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("keyValueEntry"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}