using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateBudgetAccountAsyncTests : AccountingRepositoryTestBase
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
        public void CreateBudgetAccountAsync_WhenBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBudgetAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccount"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will create a budget account (should only be used for debugging)")]
        public async Task CreateBudgetAccountAsync_WhenCalled_CreatesBudgetAccount()
        {
            IAccountingRepository sut = CreateSut();

            DateTime today = DateTime.Today;

            IAccounting accounting = await sut.GetAccountingAsync(WithExistingAccountingNumber(), today);
            IBudgetAccountGroup[] budgetAccountGroupCollection = (await sut.GetBudgetAccountGroupsAsync()).ToArray();

            IBudgetAccount budgetAccount = new BudgetAccount(accounting, WithExistingAccountNumberForBudgetAccount(), _fixture.Create<string>(), budgetAccountGroupCollection[_random.Next(0, budgetAccountGroupCollection.Length - 1)])
            {
                Description = _fixture.Create<string>()
            };

            decimal income = _random.Next(50, 70) * 1000;
            decimal expenses = _random.Next(25, 35) * 1000;

            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(-3), income, expenses));
            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(-2), income, expenses));
            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(-1), income, expenses));

            income += _random.Next(5, 10) * 1000;
            expenses += _random.Next(5, 10) * 1000;

            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today, income, expenses));
            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(1), income, expenses));
            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(2), income, expenses));

            budgetAccount.BudgetInfoCollection.Add(CreateBudgetInfo(budgetAccount, today.AddMonths(3), 0M, 0M));

            IBudgetAccount result = await sut.CreateBudgetAccountAsync(budgetAccount);

            Assert.That(result, Is.Not.Null);
        }

        private IBudgetInfo CreateBudgetInfo(IBudgetAccount budgetAccount, DateTime budgetInfoDate, decimal income, decimal expenses)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return new BudgetInfo(budgetAccount, (short)budgetInfoDate.Year, (short)budgetInfoDate.Month, income, expenses);
        }
    }
}