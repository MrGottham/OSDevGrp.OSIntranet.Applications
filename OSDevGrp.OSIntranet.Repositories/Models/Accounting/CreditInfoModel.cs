using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class CreditInfoModel : InfoModelBase
    {
        public virtual int CreditInfoIdentifier { get; set; }

        public virtual int AccountIdentifier { get; set; }

        public virtual AccountModel Account { get; set; }

        public virtual decimal Credit { get; set; }
    }

    internal static class CreditInfoModelExtensions
    {
        internal static ICreditInfo ToDomain(this CreditInfoModel creditInfoModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(creditInfoModel, nameof(creditInfoModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account = accountingModelConverter.Convert<AccountModel, IAccount>(creditInfoModel.Account);

            ICreditInfo creditInfo = new CreditInfo(account, creditInfoModel.YearMonth.Year, creditInfoModel.YearMonth.Month, creditInfoModel.Credit);
            creditInfoModel.CopyAuditInformationTo(creditInfo);
            creditInfo.SetDeletable(creditInfoModel.Deletable);

            return creditInfo;
        }

        internal static void CreateCreditInfoModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<CreditInfoModel>(entity =>
            {
                entity.HasKey(e => e.CreditInfoIdentifier);
                entity.Property(e => e.CreditInfoIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.AccountIdentifier).IsRequired();
                entity.Property(e => e.YearMonthIdentifier).IsRequired();
                entity.Property(e => e.Credit).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.AccountIdentifier, e.YearMonthIdentifier}).IsUnique();
                entity.HasOne(e => e.Account)
                    .WithMany(e => e.CreditInfos)
                    .HasForeignKey(e => e.AccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.YearMonth)
                    .WithMany(e => e.CreditInfos)
                    .HasForeignKey(e => e.YearMonthIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}