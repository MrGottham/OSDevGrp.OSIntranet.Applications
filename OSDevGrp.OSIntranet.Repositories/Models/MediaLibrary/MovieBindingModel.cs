using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MovieBindingModel : MediaBindingModelBase
	{
		public virtual int MovieBindingIdentifier { get; set; }

		public virtual int MovieIdentifier { get; set; }

		public virtual MovieModel Movie { get; set; }
	}

	internal static class MovieBindingModelExtensions
	{
		#region Methods

		internal static void CreateMovieBindingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MovieBindingModel>(entity =>
			{
				entity.HasKey(e => e.MovieBindingIdentifier);
				entity.Property(e => e.MovieBindingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.MovieIdentifier).IsRequired();
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired();
				entity.Property(e => e.Role).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => new { e.MovieIdentifier, e.MediaPersonalityIdentifier, e.Role }).IsUnique();
				entity.HasIndex(e => new { e.MovieIdentifier, e.MovieBindingIdentifier}).IsUnique();
				entity.HasIndex(e => new { e.MediaPersonalityIdentifier, e.MovieBindingIdentifier }).IsUnique();
				entity.HasOne(e => e.Movie)
					.WithMany(e => e.MovieBindings)
					.HasForeignKey(e => e.MovieIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MediaPersonality)
					.WithMany(e => e.MovieBindings)
					.HasForeignKey(e => e.MediaPersonalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}