using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class DeleteContactAccountCommandHandler : AccountIdentificationCommandHandlerBase<IDeleteContactAccountCommand>
    {
        #region Constructor

        public DeleteContactAccountCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, claimResolver, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IDeleteContactAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return AccountingRepository.DeleteContactAccountAsync(command.AccountingNumber, command.AccountNumber);
        }

        #endregion
    }
}