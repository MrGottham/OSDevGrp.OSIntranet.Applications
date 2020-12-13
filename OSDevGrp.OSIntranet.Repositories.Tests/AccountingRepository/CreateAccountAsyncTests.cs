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
    public class CreateAccountAsyncTests : AccountingRepositoryTestBase
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
        public void CreateAccountAsync_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("account"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will create an account (should only be used for debugging)")]
        public async Task CreateAccountAsync_WhenCalled_CreatesAccount()
        {
            IAccountingRepository sut = CreateSut();

            DateTime today = DateTime.Today;

            IAccounting accounting = await sut.GetAccountingAsync(WithExistingAccountingNumber(), today);
            IAccountGroup[] accountGroupCollection = (await sut.GetAccountGroupsAsync()).ToArray();

            IAccount account = new Account(accounting, WithExistingAccountNumberForAccount(), _fixture.Create<string>(), accountGroupCollection[_random.Next(0, accountGroupCollection.Length - 1)])
            {
                Description = _fixture.Create<string>()
            };

            decimal credit = _random.Next(50, 70) * 1000;

            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(-3), credit));
            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(-2), credit));
            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(-1), credit));

            credit += _random.Next(5, 10) * 1000;

            account.CreditInfoCollection.Add(CreateCreditInfo(account, today, credit));
            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(1), credit));
            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(2), credit));

            account.CreditInfoCollection.Add(CreateCreditInfo(account, today.AddMonths(3), 0M));

            IAccount result = await sut.CreateAccountAsync(account);

            Assert.That(result, Is.Not.Null);
        }

        private ICreditInfo CreateCreditInfo(IAccount account, DateTime creditInfoDate, decimal credit)
        {
            NullGuard.NotNull(account, nameof(account));

            return new CreditInfo(account, (short) creditInfoDate.Year, (short) creditInfoDate.Month, credit);
        }
    }
}