﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers
{
    internal class CreateBudgetAccountCommandHandler : AccountIdentificationCommandHandlerBase<ICreateBudgetAccountCommand>
    {
        #region Constructor

        public CreateBudgetAccountCommandHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, ICommonRepository commonRepository) 
            : base(validator, claimResolver, accountingRepository, commonRepository)
        {
        }

        #endregion

        #region Methods

        protected override Task ManageRepositoryAsync(ICreateBudgetAccountCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IBudgetAccount budgetAccount = command.ToDomain(AccountingRepository);

            return AccountingRepository.CreateBudgetAccountAsync(budgetAccount);
        }

        #endregion
    }
}