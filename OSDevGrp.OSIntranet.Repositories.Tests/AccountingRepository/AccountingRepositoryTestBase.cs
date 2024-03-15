using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    public abstract class AccountingRepositoryTestBase : DatabaseRepositoryTestBase
    {
        #region Private variables

        private static IOptions<AccountingRepositoryTestOptions> _accountingRepositoryTestOptions;
        private static Random _random;

        #endregion

        protected IAccountingRepository CreateSut()
        {
            return new Repositories.AccountingRepository(CreateTestRepositoryContext(), CreateLoggerFactory(), CreateEventPublisher());
        }

        protected int WithExistingAccountingNumber()
        {
            return GetAccountingRepositoryTestOptions().Value.ExistingAccountingNumber;
        }

        protected string WithExistingAccountNumberForAccount()
        {
            return GetAccountingRepositoryTestOptions().Value.ExistingAccountNumberForAccount;
        }

        protected string WithExistingAccountNumberForBudgetAccount()
        {
            return GetAccountingRepositoryTestOptions().Value.ExistingAccountNumberForBudgetAccount;
        }

        protected string WithExistingAccountNumberForContactAccount()
        {
            return GetAccountingRepositoryTestOptions().Value.ExistingAccountNumberForContactAccount;
        }

        protected static int WithNonExistingAccountingNumber()
        {
            return GetRandomizer().Next(100, 256);
        }

        protected static string WithNonExistingAccountNumber()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        private IOptions<AccountingRepositoryTestOptions> GetAccountingRepositoryTestOptions()
        {
            lock (SyncRoot)
            {
                if (_accountingRepositoryTestOptions != null)
                {
                    return _accountingRepositoryTestOptions;
                }

                return _accountingRepositoryTestOptions = Microsoft.Extensions.Options.Options.Create(CreateTestConfiguration().GetSection("TestData:Accounting").Get<AccountingRepositoryTestOptions>());
            }
        }

        private static Random GetRandomizer()
        {
            lock (SyncRoot)
            {
                if (_random != null)
                {
                    return _random;
                }

                Fixture fixture = new Fixture();
                _random = new Random(fixture.Create<int>());

                return _random;
            }
        }
    }
}