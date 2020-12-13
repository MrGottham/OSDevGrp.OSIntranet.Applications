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
    public class UpdateAccountAsyncTests : AccountingRepositoryTestBase
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
        public void UpdateAccountAsync_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("account"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will update an account (should only be used for debugging)")]
        public async Task UpdateAccountAsync_WhenCalled_UpdatesAccount()
        {
            IAccountingRepository sut = CreateSut();

            IAccountGroup[] accountGroupCollection = (await sut.GetAccountGroupsAsync()).ToArray();

            DateTime today = DateTime.Today;

            IAccount account = await sut.GetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForAccount(), today);
            account.Description = _fixture.Create<string>();
            account.Note = _fixture.Create<string>();
            account.AccountGroup = accountGroupCollection[_random.Next(0, accountGroupCollection.Length - 1)];

            decimal credit = _random.Next(50, 70) * 1000;
            
            ICreditInfo creditInfo = account.CreditInfoCollection.Single(m => m.Year == today.AddMonths(-3).Year && m.Month == today.AddMonths(-3).Month);
            creditInfo.Credit = credit;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = credit;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = credit;

            credit +=  _random.Next(5, 10) * 1000;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = credit;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = credit;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = credit;

            creditInfo = account.CreditInfoCollection.Next(creditInfo);
            creditInfo.Credit = 0M;

            IAccount result = await sut.UpdateAccountAsync(account);

            Assert.That(result, Is.Not.Null);
        }
    }
}