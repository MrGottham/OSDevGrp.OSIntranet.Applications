using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class DeleteAccountGroupCommand : AccountGroupIdentificationCommandBase, IDeleteAccountGroupCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository) 
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository));

            IAccountGroup accountGroup = null;
            Task<IAccountGroup> accountGroupGetter = Task.Run(async () => accountGroup ?? (accountGroup = await accountingRepository.GetAccountGroupAsync(Number)));

            return base.Validate(validator, accountingRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await accountGroupGetter != null), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => accountGroupGetter, GetType(), nameof(Number));
        }

        #endregion
    }
}