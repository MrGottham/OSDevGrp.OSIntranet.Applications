using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
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

            return base.Validate(validator, accountingRepository)
                .Object.ShouldBeKnownValue(Number, number => Task.Run(async () => await GetAccountGroup(accountingRepository) != null), GetType(), nameof(Number))
                .Object.ShouldBeDeletable(Number, number => GetAccountGroup(accountingRepository), GetType(), nameof(Number));
        }

        #endregion
    }
}