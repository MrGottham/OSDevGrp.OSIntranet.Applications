using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class BudgetAccountDataCommandBase : AccountCoreDataCommandBase<IBudgetAccount>, IBudgetAccountDataCommand
    {
        #region Private variables

        private IBudgetAccountGroup _budgetAccountGroup;

        #endregion

        #region Properties

        public int BudgetAccountGroupNumber { get; set; }

        public IEnumerable<IBudgetInfoCommand> BudgetInfoCollection { get; set; } = Array.Empty<IBudgetInfoCommand>();

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));
            
            BudgetInfoCollection?.ForAll(budgetInfo => budgetInfo.Validate(validator));

            return base.Validate(validator, accountingRepository, commonRepository)
                .ValidateAccountGroupIdentifier(BudgetAccountGroupNumber, GetType(), nameof(BudgetAccountGroupNumber))
                .Object.ShouldBeKnownValue(BudgetAccountGroupNumber, budgetAccountGroupNumber => Task.Run(async () => await GetBudgetAccountGroupAsync(accountingRepository) != null), GetType(), nameof(BudgetAccountGroupNumber))
                .Object.ShouldNotBeNull(BudgetInfoCollection, GetType(), nameof(BudgetInfoCollection));
        }

        public override IBudgetAccount ToDomain(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = GetAccountingAsync(accountingRepository).GetAwaiter().GetResult();
            IBudgetAccountGroup budgetAccountGroup = GetBudgetAccountGroupAsync(accountingRepository).GetAwaiter().GetResult();

            IBudgetAccount budgetAccount = new BudgetAccount(accounting, AccountNumber, AccountName, budgetAccountGroup)
            {
                Description = Description,
                Note = Note
            };

            IBudgetInfo[] budgetInfoCollection = (BudgetInfoCollection ?? Array.Empty<IBudgetInfoCommand>())
                .AsParallel()
                .Select(budgetInfo => budgetInfo.ToDomain(budgetAccount))
                .ToArray();
            budgetAccount.BudgetInfoCollection.Add(budgetInfoCollection);

            return budgetAccount;
        }

        protected Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return Task.FromResult(BudgetAccountGroupNumber.GetBudgetAccountGroup(accountingRepository, ref _budgetAccountGroup));
        }

        #endregion
    }
}