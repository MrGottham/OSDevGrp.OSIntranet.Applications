using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class CreateNationalityAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateNationalityAsync_WhenNationalityIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateNationalityAsync(null)))!;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("nationality"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}