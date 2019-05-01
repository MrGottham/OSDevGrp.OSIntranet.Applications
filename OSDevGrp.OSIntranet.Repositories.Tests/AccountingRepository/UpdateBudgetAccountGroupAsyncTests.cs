using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class UpdateBudgetAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateBudgetAccountGroupAsync_WhenBudgetAccountGroupIsNull_ThrownsArgumentException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccountGroupAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccountGroup"));
        }
    }
}