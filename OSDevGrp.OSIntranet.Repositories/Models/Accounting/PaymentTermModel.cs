using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class PaymentTermModel : AuditModelBase
    {
        public virtual int PaymentTermIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Deletable { get; set; }

        public virtual List<ContactAccountModel> ContactAccounts { get; set; }

        public virtual List<ContactSupplementModel> ContactSupplements { get; set; }
    }

    internal static class PaymentTermModelExtensions
    {
        internal static IPaymentTerm ToDomain(this PaymentTermModel paymentTermModel)
        {
            NullGuard.NotNull(paymentTermModel, nameof(paymentTermModel));

            IPaymentTerm paymentTerm = new PaymentTerm(paymentTermModel.PaymentTermIdentifier, paymentTermModel.Name);
            paymentTerm.AddAuditInformation(paymentTermModel.CreatedUtcDateTime, paymentTermModel.CreatedByIdentifier, paymentTermModel.ModifiedUtcDateTime, paymentTermModel.ModifiedByIdentifier);
            paymentTerm.SetDeletable(paymentTermModel.Deletable);

            return paymentTerm;
        }

        internal static void CreatePaymentTermModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<PaymentTermModel>(entity =>
            {
                entity.HasKey(e => e.PaymentTermIdentifier);
                entity.Property(e => e.PaymentTermIdentifier).IsRequired();
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