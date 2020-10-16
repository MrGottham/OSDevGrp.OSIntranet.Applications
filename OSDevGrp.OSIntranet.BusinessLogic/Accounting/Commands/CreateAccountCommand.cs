using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class CreateAccountCommand : AccountDataCommandBase, ICreateAccountCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .Object.ShouldBeUnknownValue(AccountNumber, accountNumber => Task.Run(async () => await AccountExistsAsync(accountingRepository) == false), GetType(), nameof(AccountNumber));
        }

        #endregion
    }
}