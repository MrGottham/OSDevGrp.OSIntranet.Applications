using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class CreateLetterHeadAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateLetterHeadAsync_WhenLetterHeadIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateLetterHeadAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("letterHead"));
        }
    }
}