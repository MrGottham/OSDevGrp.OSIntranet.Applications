using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class KeyValueEntryModel : AuditModelBase
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public virtual bool Deletable { get; set; }
    }

    internal static class KeyValueEntryModelExtension
    {
        internal static IKeyValueEntry ToDomain(this KeyValueEntryModel keyValueEntryModel)
        {
            NullGuard.NotNull(keyValueEntryModel, nameof(keyValueEntryModel));

            IKeyValueEntry keyValueEntry = KeyValueEntry.Create(keyValueEntryModel.Key, keyValueEntryModel.Value);
            keyValueEntry.AddAuditInformation(keyValueEntryModel.CreatedUtcDateTime, keyValueEntryModel.CreatedByIdentifier, keyValueEntryModel.ModifiedUtcDateTime, keyValueEntryModel.ModifiedByIdentifier);
            keyValueEntry.SetDeletable(keyValueEntryModel.Deletable);

            return keyValueEntry;
        }

        internal static void CreateKeyValueEntryModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<KeyValueEntryModel>(entity =>
            {
                entity.HasKey(e => e.Key);
                entity.Property(e => e.Key).HasColumnType("VARBINARY(256)").IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.Value).HasColumnType("TEXT").IsRequired().IsUnicode();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}