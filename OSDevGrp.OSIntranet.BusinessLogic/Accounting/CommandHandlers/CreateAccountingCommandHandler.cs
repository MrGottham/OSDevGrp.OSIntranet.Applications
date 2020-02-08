using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class CreateAccountingCommandHandler : AccountingIdentificationCommandHandlerBase<ICreateAccountingCommand>
    {
        #region Constructor

        public CreateAccountingCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateAccountingCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IAccounting accounting = command.ToDomain(CommonRepository);

            await AccountingRepository.CreateAccountingAsync(accounting);
        }

        #endregion
    }
}