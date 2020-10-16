using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountDataCommandBase : AccountCoreDataCommandBase<IAccount>, IAccountDataCommand
    {
        #region Private variables

        private IAccountGroup _accountGroup;

        #endregion

        #region Properties

        public int AccountGroupNumber { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .ValidateAccountGroupIdentifier(AccountGroupNumber, GetType(), nameof(AccountGroupNumber))
                .Object.ShouldBeKnownValue(AccountGroupNumber, accountGroupNumber => Task.Run(async () => await GetAccountGroupAsync(accountingRepository) != null), GetType(), nameof(AccountGroupNumber));
        }

        public override IAccount ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            IAccountGroup accountGroup = GetAccountGroupAsync(accountingRepository).GetAwaiter().GetResult();

            return new Account(accounting, AccountNumber, AccountName, accountGroup)
            {
                Description = Description,
                Note = Note
            };
        }

        protected Task<IAccountGroup> GetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(AccountGroupNumber.GetAccountGroup(accountingRepository, ref _accountGroup));
        }

        #endregion
    }
}