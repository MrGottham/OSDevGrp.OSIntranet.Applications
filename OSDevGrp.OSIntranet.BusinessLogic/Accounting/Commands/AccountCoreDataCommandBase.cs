using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountCoreDataCommandBase<TAccount> : AccountIdentificationCommandBase, IAccountCoreDataCommand<TAccount> where TAccount : IAccountBase
    {
        #region Properties

        public string AccountName { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .String.ShouldNotBeNullOrWhiteSpace(AccountName, GetType(), nameof(AccountName))
                .String.ShouldHaveMinLength(AccountName, 1, GetType(), nameof(AccountName))
                .String.ShouldHaveMaxLength(AccountName, 256, GetType(), nameof(AccountName))
                .String.ShouldHaveMinLength(Description, 1, GetType(), nameof(Description), true)
                .String.ShouldHaveMaxLength(Description, 512, GetType(), nameof(Description), true)
                .String.ShouldHaveMinLength(Note, 1, GetType(), nameof(Note), true)
                .String.ShouldHaveMaxLength(Note, 4096, GetType(), nameof(Note), true);
        }

        public abstract TAccount ToDomain(IAccountingRepository accountingRepository);

        #endregion
    }
}