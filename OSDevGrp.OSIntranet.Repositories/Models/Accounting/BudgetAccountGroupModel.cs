using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountGroupModel : AccountGroupModelBase
    {
        public virtual int BudgetAccountGroupIdentifier { get; set; }
    }

    internal static class BudgetAccountGroupModelExtensions
    {
        internal static IBudgetAccountGroup ToDomain(this BudgetAccountGroupModel budgetAccountGroupModel)
        {
            NullGuard.NotNull(budgetAccountGroupModel, nameof(budgetAccountGroupModel));

            IBudgetAccountGroup budgetAccountGroup = new BudgetAccountGroup(budgetAccountGroupModel.BudgetAccountGroupIdentifier, budgetAccountGroupModel.Name);
            budgetAccountGroup.AddAuditInformations(budgetAccountGroupModel.CreatedUtcDateTime, budgetAccountGroupModel.CreatedByIdentifier, budgetAccountGroupModel.ModifiedUtcDateTime, budgetAccountGroupModel.ModifiedByIdentifier);

            return budgetAccountGroup;
        }
    }
}
