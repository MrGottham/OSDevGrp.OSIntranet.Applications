using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class UpdateAccountingCommandHandler : AccountingIdentificationCommandHandlerBase<IUpdateAccountingCommand>
    {
        #region Constructor

        public UpdateAccountingCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
            : base(validator, claimResolver, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateAccountingCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IAccounting accounting = command.ToDomain(CommonRepository);

            await AccountingRepository.UpdateAccountingAsync(accounting);
        }

        #endregion
    }
}