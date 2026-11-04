using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateBudgetAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateBudgetAccountGroupAsync_WhenBudgetAccountGroupIsNull_ThrowsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBudgetAccountGroupAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("budgetAccountGroup"));
        }
    }
}