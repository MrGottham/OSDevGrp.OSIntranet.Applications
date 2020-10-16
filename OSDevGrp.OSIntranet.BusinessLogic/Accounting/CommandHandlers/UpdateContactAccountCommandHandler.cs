using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class UpdateContactAccountCommandHandler : AccountIdentificationCommandHandlerBase<IUpdateContactAccountCommand>
    {
        #region Constructor

        public UpdateContactAccountCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IUpdateContactAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IContactAccount contactAccount = command.ToDomain(AccountingRepository);

            return AccountingRepository.UpdateContactAccountAsync(contactAccount);
        }

        #endregion
    }
}