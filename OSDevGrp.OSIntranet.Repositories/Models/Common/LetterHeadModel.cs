using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class LetterHeadModel : AuditModelBase
    {
        public virtual int LetterHeadIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string Line1 { get; set; }

        public virtual string Line2 { get; set; }

        public virtual string Line3 { get; set; }

        public virtual string Line4 { get; set; }

        public virtual string Line5 { get; set; }

        public virtual string Line6 { get; set; }

        public virtual string Line7 { get; set; }

        public virtual string CompanyIdentificationNumber { get ; set; }

        public virtual bool Deletable { get; set; }
    }

    internal static class LetterHeadModelExtensions
    {
        internal static ILetterHead ToDomain(this LetterHeadModel letterHeadModel)
        {
            NullGuard.NotNull(letterHeadModel, nameof(letterHeadModel));

            ILetterHead letterHead = new LetterHead(letterHeadModel.LetterHeadIdentifier, letterHeadModel.Name, letterHeadModel.Line1)
            {
                Line2 = letterHeadModel.Line2,
                Line3 = letterHeadModel.Line3,
                Line4 = letterHeadModel.Line4,
                Line5 = letterHeadModel.Line5,
                Line6 = letterHeadModel.Line6,
                Line7 = letterHeadModel.Line7,
                CompanyIdentificationNumber = letterHeadModel.CompanyIdentificationNumber
            };
            letterHead.AddAuditInformation(letterHeadModel.CreatedUtcDateTime, letterHeadModel.CreatedByIdentifier, letterHeadModel.ModifiedUtcDateTime, letterHeadModel.ModifiedByIdentifier);
            letterHead.SetDeletable(letterHeadModel.Deletable);

            return letterHead;
        }

        internal static void CreateLetterHeadModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<LetterHeadModel>(entity =>
            {
                entity.HasKey(e => e.LetterHeadIdentifier);
                entity.Property(e => e.LetterHeadIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.Line1).IsRequired().IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line2).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line3).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line4).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line5).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line6).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.Line7).IsRequired(false).IsUnicode().HasMaxLength(64);
                entity.Property(e => e.CompanyIdentificationNumber).IsRequired(false).IsUnicode().HasMaxLength(32);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}