using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class UpdateBudgetAccountAsyncTests : AccountingRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateBudgetAccountAsync_WhenBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccount"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will update a budget account (should only be used for debugging)")]
        public async Task UpdateBudgetAccountAsync_WhenCalled_UpdatesBudgetAccount()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccountGroup[] budgetAccountGroupCollection = (await sut.GetBudgetAccountGroupsAsync()).ToArray();

            DateTime today = DateTime.Today;

            IBudgetAccount budgetAccount = await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount(), today);
            budgetAccount.Description = _fixture.Create<string>();
            budgetAccount.Note = _fixture.Create<string>();
            budgetAccount.BudgetAccountGroup = budgetAccountGroupCollection[_random.Next(0, budgetAccountGroupCollection.Length - 1)];

            decimal income = _random.Next(50, 70) * 1000;
            decimal expenses = _random.Next(25, 35) * 1000;

            IBudgetInfo budgetInfo = budgetAccount.BudgetInfoCollection.Single(m => m.Year == today.AddMonths(-3).Year && m.Month == today.AddMonths(-3).Month);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            income += _random.Next(5, 10) * 1000;
            expenses += _random.Next(5, 10) * 1000;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = income;
            budgetInfo.Expenses = expenses;

            budgetInfo = budgetAccount.BudgetInfoCollection.Next(budgetInfo);
            budgetInfo.Income = 0M;
            budgetInfo.Expenses = 0M;

            IBudgetAccount result = await sut.UpdateBudgetAccountAsync(budgetAccount);

            Assert.That(result, Is.Not.Null);
        }
    }
}