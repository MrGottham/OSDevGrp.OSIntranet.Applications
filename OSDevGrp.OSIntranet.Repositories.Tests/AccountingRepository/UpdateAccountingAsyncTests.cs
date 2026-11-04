using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class UpdateAccountingAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task UpdateAccountingAsync_WhenAccountingIsNull_ThrowsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccountingAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("accounting"));
        }
    }
}