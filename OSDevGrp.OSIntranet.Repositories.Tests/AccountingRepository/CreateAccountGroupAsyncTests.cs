using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateAccountGroupAsync_WhenAccountGroupIsNull_ThrowsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateAccountGroupAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("accountGroup"));
        }
    }
}