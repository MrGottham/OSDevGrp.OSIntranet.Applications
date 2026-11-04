using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class UpdateNationalityAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task UpdateNationalityAsync_WhenNationalityIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateNationalityAsync(null)))!;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("nationality"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}