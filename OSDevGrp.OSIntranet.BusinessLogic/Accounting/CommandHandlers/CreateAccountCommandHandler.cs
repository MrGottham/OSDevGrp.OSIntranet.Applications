using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class CreateAccountCommandHandler : AccountIdentificationCommandHandlerBase<ICreateAccountCommand>
    {
        #region Constructor

        public CreateAccountCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(ICreateAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IAccount account = command.ToDomain(AccountingRepository);

            return AccountingRepository.CreateAccountAsync(account);
        }

        #endregion
    }
}