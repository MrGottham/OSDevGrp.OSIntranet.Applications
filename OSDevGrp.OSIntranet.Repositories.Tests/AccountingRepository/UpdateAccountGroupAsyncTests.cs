using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class UpdateAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task UpdateAccountGroupAsync_WhenAccountGroupIsNull_ThrowsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccountGroupAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("accountGroup"));
        }
    }
}