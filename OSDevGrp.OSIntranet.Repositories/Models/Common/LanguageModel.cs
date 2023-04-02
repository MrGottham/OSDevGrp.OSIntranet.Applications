using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
	internal class LanguageModel : GenericCategoryModelBase
    {
        public int LanguageIdentifier { get; set; }

        public virtual List<MovieModel> Movies { get; set; }

        public virtual List<BookModel> Books { get; set; }
    }

    internal static class LanguageModelExtensions
    {
        #region Methods

        internal static ILanguage ToDomain(this LanguageModel languageModel)
        {
            NullGuard.NotNull(languageModel, nameof(languageModel));

            ILanguage language = new Language(languageModel.LanguageIdentifier, languageModel.Name);
            language.SetDeletable(languageModel.Deletable);
            language.AddAuditInformation(languageModel.CreatedUtcDateTime, languageModel.CreatedByIdentifier, languageModel.ModifiedUtcDateTime, languageModel.ModifiedByIdentifier);

            return language;
        }

        internal static void CreateLanguageModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<LanguageModel>(entity =>
            {
                entity.HasKey(e => e.LanguageIdentifier);
                entity.Property(e => e.LanguageIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }

        #endregion
    }
}