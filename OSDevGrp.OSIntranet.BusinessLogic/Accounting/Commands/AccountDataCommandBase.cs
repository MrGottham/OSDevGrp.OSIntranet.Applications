using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountDataCommandBase : AccountCoreDataCommandBase<IAccount>, IAccountDataCommand
    {
        #region Private variables

        private IAccountGroup _accountGroup;

        #endregion

        #region Properties

        public int AccountGroupNumber { get; set; }

        public IEnumerable<ICreditInfoCommand> CreditInfoCollection { get; set; } = Array.Empty<ICreditInfoCommand>();

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
	            .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            foreach (ICreditInfoCommand creditInfo in CreditInfoCollection ?? Array.Empty<ICreditInfoCommand>())
            {
                creditInfo.Validate(validator);
            }

            return base.Validate(validator, claimResolver, accountingRepository, commonRepository)
                .ValidateAccountGroupIdentifier(AccountGroupNumber, GetType(), nameof(AccountGroupNumber))
                .Object.ShouldBeKnownValue(AccountGroupNumber, accountGroupNumber => Task.Run(async () => await GetAccountGroupAsync(accountingRepository) != null), GetType(), nameof(AccountGroupNumber))
                .Object.ShouldNotBeNull(CreditInfoCollection, GetType(), nameof(CreditInfoCollection));
        }

        public override IAccount ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            IAccountGroup accountGroup = GetAccountGroupAsync(accountingRepository).GetAwaiter().GetResult();

            IAccount account = new Account(accounting, AccountNumber, AccountName, accountGroup)
            {
                Description = Description,
                Note = Note
            };

            ICreditInfo[] creditInfoCollection = (CreditInfoCollection ?? Array.Empty<ICreditInfoCommand>())
                .AsParallel()
                .Select(creditInfo => creditInfo.ToDomain(account))
                .ToArray();
            account.CreditInfoCollection.Add(creditInfoCollection);

            return account;
        }

        protected Task<IAccountGroup> GetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(AccountGroupNumber.GetAccountGroup(accountingRepository, ref _accountGroup));
        }

        #endregion
    }
}