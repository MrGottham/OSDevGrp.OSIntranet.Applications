using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public abstract class AccountIdentificationCommandHandlerBase<T> : AccountingIdentificationCommandHandlerBase<T> where T : IAccountIdentificationCommand
    {
        #region Constructor

        protected AccountIdentificationCommandHandlerBase(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion
    }
}