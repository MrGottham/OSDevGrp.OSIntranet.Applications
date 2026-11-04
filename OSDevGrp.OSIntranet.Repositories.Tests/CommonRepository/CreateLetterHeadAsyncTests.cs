using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class CreateLetterHeadAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateLetterHeadAsync_WhenLetterHeadIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateLetterHeadAsync(null)))!;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("letterHead"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}