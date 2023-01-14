using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public class CreateAccountingCommand : AccountingCommandBase, ICreateAccountingCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
	            .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, claimResolver, accountingRepository, commonRepository)
                .Object.ShouldBeUnknownValue(AccountingNumber, accountingNumber => Task.Run(async () => await AccountingExistsAsync(accountingRepository) == false), GetType(), nameof(AccountingNumber));
        }

        protected override bool EvaluateNecessaryPermission(IClaimResolver claimResolver)
        {
	        NullGuard.NotNull(claimResolver, nameof(claimResolver));

	        return claimResolver.IsAccountingCreator();
        }

        #endregion
    }
}