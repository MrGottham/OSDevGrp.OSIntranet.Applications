using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountGroupModel : AccountGroupModelBase
    {
        public virtual int BudgetAccountGroupIdentifier { get; set; }

        public virtual List<BudgetAccountModel> BudgetAccounts { get; set; }
    }

    internal static class BudgetAccountGroupModelExtensions
    {
        internal static IBudgetAccountGroup ToDomain(this BudgetAccountGroupModel budgetAccountGroupModel)
        {
            NullGuard.NotNull(budgetAccountGroupModel, nameof(budgetAccountGroupModel));

            IBudgetAccountGroup budgetAccountGroup = new BudgetAccountGroup(budgetAccountGroupModel.BudgetAccountGroupIdentifier, budgetAccountGroupModel.Name);
            budgetAccountGroup.AddAuditInformation(budgetAccountGroupModel.CreatedUtcDateTime, budgetAccountGroupModel.CreatedByIdentifier, budgetAccountGroupModel.ModifiedUtcDateTime, budgetAccountGroupModel.ModifiedByIdentifier);
            budgetAccountGroup.SetDeletable(budgetAccountGroupModel.Deletable);

            return budgetAccountGroup;
        }

        internal static void CreateBudgetAccountGroupModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<BudgetAccountGroupModel>(entity =>
            {
                entity.HasKey(e => e.BudgetAccountGroupIdentifier);
                entity.Property(e => e.BudgetAccountGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}