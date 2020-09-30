using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
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

            IAccounting accounting = await sut.GetAccountingAsync(WithExistingAccountingNumber(), DateTime.Today);
            IBudgetAccountGroup[] budgetAccountGroupCollection = (await sut.GetBudgetAccountGroupsAsync()).ToArray();

            IBudgetAccount budgetAccount = new BudgetAccount(accounting, WithExistingAccountNumberForBudgetAccount(), _fixture.Create<string>(), budgetAccountGroupCollection[_random.Next(0, budgetAccountGroupCollection.Length - 1)])
            {
                Description = _fixture.Create<string>()
            };
            IBudgetAccount result = await sut.CreateBudgetAccountAsync(budgetAccount);

            Assert.That(result, Is.Not.Null);
        }
    }
}