﻿using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    public class UpdateBudgetAccountCommandHandler : AccountIdentificationCommandHandlerBase<IUpdateBudgetAccountCommand>
    {
        #region Constructor

        public UpdateBudgetAccountCommandHandler(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(IUpdateBudgetAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IBudgetAccount budgetAccount = command.ToDomain(AccountingRepository);

            return AccountingRepository.UpdateBudgetAccountAsync(budgetAccount);
        }

        #endregion
    }
}