using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountingModel : AuditModelBase
    {
        public virtual int AccountingIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual int LetterHeadIdentifier { get; set; }

        public virtual LetterHeadModel LetterHead { get; set; }

        public virtual BalanceBelowZeroType BalanceBelowZero { get; set; }

        public virtual int BackDating { get; set; }

        public virtual bool Deletable { get; set; }
    }

    internal static class AccountingModelExtensions
    {
        internal static IAccounting ToDomain(this AccountingModel accountingModel, IConverter commonModelConverter)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(commonModelConverter, nameof(commonModelConverter));

            ILetterHead letterHead = commonModelConverter.Convert<LetterHeadModel, ILetterHead>(accountingModel.LetterHead);

            IAccounting accounting = new Domain.Accounting.Accounting(accountingModel.AccountingIdentifier, accountingModel.Name, letterHead, accountingModel.BalanceBelowZero, accountingModel.BackDating);
            accounting.AddAuditInformation(accountingModel.CreatedUtcDateTime, accountingModel.CreatedByIdentifier, accountingModel.ModifiedUtcDateTime, accountingModel.ModifiedByIdentifier);
            accounting.SetDeletable(accountingModel.Deletable);

            return accounting;
        }
 
        internal static void CreateAccountingModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<AccountingModel>(entity =>
            {
                entity.HasKey(e => e.AccountingIdentifier);
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.LetterHeadIdentifier).IsRequired();
                entity.Property(e => e.BalanceBelowZero).IsRequired();
                entity.Property(e => e.BackDating).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.LetterHeadIdentifier, e.AccountingIdentifier}).IsUnique();
                entity.HasOne(e => e.LetterHead)
                    .WithMany(e => e.Accountings)
                    .HasForeignKey(e => e.LetterHeadIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}