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

            return new BudgetAccountGroup(budgetAccountGroupModel.BudgetAccountGroupIdentifier, budgetAccountGroupModel.Name);
        }
    }
}
