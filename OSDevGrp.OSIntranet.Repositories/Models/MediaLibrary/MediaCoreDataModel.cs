using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MediaCoreDataModel : AuditModelBase
    {
        public virtual int MediaCoreDataIdentifier { get; set; }

        public virtual string Title { get; set; }

        public virtual string Subtitle { get; set; }

        public virtual string Description { get; set; }

        public virtual string Details { get; set; }

        public virtual int MediaTypeIdentifier { get; set; }

        public virtual MediaTypeModel MediaType { get; set; }

        public virtual short? Published { get; set; }

        public virtual string Url { get; set; }

        public virtual string Image { get; set; }

        public virtual int? MovieIdentifier { get; set; }

        public virtual MovieModel Movie { get; set; }

        public virtual int? MusicIdentifier { get; set; }

        public virtual MusicModel Music { get; set; }

        public virtual int? BookIdentifier { get; set; }

        public virtual BookModel Book { get; set; }
    }

    internal static class MediaCoreDataModelExtensions
	{
		#region Methods

		internal static void CreateMediaCoreDataModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MediaCoreDataModel>(entity =>
			{
				entity.HasKey(e => e.MediaCoreDataIdentifier);
				entity.Property(e => e.MediaCoreDataIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.Title).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.Subtitle).IsRequired(false).IsUnicode().HasMaxLength(256);
				entity.Property(e => e.Description).IsRequired(false).IsUnicode().HasMaxLength(512);
				entity.Property(e => e.Details).HasColumnType("TEXT").IsRequired(false).IsUnicode();
				entity.Property(e => e.MediaTypeIdentifier).IsRequired();
				entity.Property(e => e.Published).IsRequired(false);
				entity.Property(e => e.Url).IsRequired(false).IsUnicode().HasMaxLength(256);
				entity.Property(e => e.Image).HasColumnType("TEXT").IsRequired(false).IsUnicode();
				entity.Property(e => e.MovieIdentifier).IsRequired(false);
				entity.Property(e => e.MusicIdentifier).IsRequired(false);
				entity.Property(e => e.BookIdentifier).IsRequired(false);
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.HasIndex(e => new { e.Title, e.Subtitle, e.MediaTypeIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MediaTypeIdentifier, e.MediaCoreDataIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MovieIdentifier, e.MediaCoreDataIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MusicIdentifier, e.MediaCoreDataIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.BookIdentifier, e.MediaCoreDataIdentifier }).IsUnique();
				entity.HasOne(e => e.MediaType)
					.WithMany(e => e.CoreData)
					.HasForeignKey(e => e.MediaTypeIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

        #endregion
	}
}