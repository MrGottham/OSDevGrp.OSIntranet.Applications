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

            IBudgetAccount budgetAccount = await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount(), DateTime.Today);
            budgetAccount.Description = _fixture.Create<string>();
            budgetAccount.Note = _fixture.Create<string>();
            budgetAccount.BudgetAccountGroup = budgetAccountGroupCollection[_random.Next(0, budgetAccountGroupCollection.Length - 1)];

            IBudgetAccount result = await sut.UpdateBudgetAccountAsync(budgetAccount);

            Assert.That(result, Is.Not.Null);
        }
    }
}