using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateAccountingAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateAccountingAsync_WhenAccountingIsNull_ThrowsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateAccountingAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("accounting"));
        }
    }
}