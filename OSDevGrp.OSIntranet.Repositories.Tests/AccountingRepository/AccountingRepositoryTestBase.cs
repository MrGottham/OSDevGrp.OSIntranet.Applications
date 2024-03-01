using AutoFixture;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    public abstract class AccountingRepositoryTestBase : DatabaseRepositoryTestBase
    {
        #region Private variables

        private static int? _existingAccountingNumber;
        private static string _existingAccountNumberForAccount;
        private static string _existingAccountNumberForBudgetAccount;
        private static string _existingAccountNumberForContactAccount;
        private static Random _random;

        #endregion

        protected IAccountingRepository CreateSut()
        {
            return new Repositories.AccountingRepository(CreateTestRepositoryContext(), CreateLoggerFactory(), CreateEventPublisher());
        }

        protected int WithExistingAccountingNumber()
        {
            lock (SyncRoot)
            {
                if (_existingAccountingNumber.HasValue)
                {
                    return _existingAccountingNumber.Value;
                }

                IConfiguration configuration = CreateTestConfiguration();
                return (_existingAccountingNumber = int.Parse(configuration["TestData:Accounting:ExistingAccountingNumber"])).Value;
            }
        }

        protected string WithExistingAccountNumberForAccount()
        {
            lock (SyncRoot)
            {
                if (string.IsNullOrWhiteSpace(_existingAccountNumberForAccount) == false)
                {
                    return _existingAccountNumberForAccount;
                }

                IConfiguration configuration = CreateTestConfiguration();
                return _existingAccountNumberForAccount = configuration["TestData:Accounting:ExistingAccountNumberForAccount"];
            }
        }

        protected string WithExistingAccountNumberForBudgetAccount()
        {
            lock (SyncRoot)
            {
                if (string.IsNullOrWhiteSpace(_existingAccountNumberForBudgetAccount) == false)
                {
                    return _existingAccountNumberForBudgetAccount;
                }

                IConfiguration configuration = CreateTestConfiguration();
                return _existingAccountNumberForBudgetAccount = configuration["TestData:Accounting:ExistingAccountNumberForBudgetAccount"];
            }
        }

        protected string WithExistingAccountNumberForContactAccount()
        {
            lock (SyncRoot)
            {
                if (string.IsNullOrWhiteSpace(_existingAccountNumberForContactAccount) == false)
                {
                    return _existingAccountNumberForContactAccount;
                }

                IConfiguration configuration = CreateTestConfiguration();
                return _existingAccountNumberForContactAccount = configuration["TestData:Accounting:ExistingAccountNumberForContactAccount"];
            }
        }

        protected static int WithNonExistingAccountingNumber()
        {
            lock (SyncRoot)
            {
                if (_random == null)
                {
                    Fixture fixture = new Fixture();
                    _random = new Random(fixture.Create<int>());
                }

                return _random.Next(100, 256);
            }
        }

        protected static string WithNonExistingAccountNumber()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }
}