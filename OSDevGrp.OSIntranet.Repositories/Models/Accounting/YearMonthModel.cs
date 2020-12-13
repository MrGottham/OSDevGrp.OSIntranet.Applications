using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class YearMonthModel : AuditModelBase
    {
        public virtual int YearMonthIdentifier { get; set; }

        public virtual short Year { get; set; }

        public virtual short Month { get; set; }

        public virtual List<CreditInfoModel> CreditInfos { get; set; }

        public virtual List<BudgetInfoModel> BudgetInfos { get; set; }
    }

    internal static class YearMonthModelExtensions
    {
        internal static void CreateYearMonthModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<YearMonthModel>(entity =>
            {
                entity.HasKey(e => e.YearMonthIdentifier);
                entity.Property(e => e.YearMonthIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Month).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => new {e.Year, e.Month}).IsUnique();
            });
        }
    }
}