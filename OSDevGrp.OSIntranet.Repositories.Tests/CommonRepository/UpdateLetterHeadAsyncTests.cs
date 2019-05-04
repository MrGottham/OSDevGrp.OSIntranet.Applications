using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class UpdateLetterHeadAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateLetterHeadAsync_WhenLetterHeadIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateLetterHeadAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("letterHead"));
        }
    }
}