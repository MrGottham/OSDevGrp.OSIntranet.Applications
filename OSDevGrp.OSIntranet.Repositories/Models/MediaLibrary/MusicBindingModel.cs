using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MusicBindingModel : MediaBindingModelBase
	{
		public virtual int MusicBindingIdentifier { get; set; }

		public virtual int MusicIdentifier { get; set; }

		public virtual MusicModel Music { get; set; }
	}

	internal static class MusicBindingModelExtensions
	{
		#region Methods

		internal static void CreateMusicBindingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MusicBindingModel>(entity =>
			{
				entity.HasKey(e => e.MusicBindingIdentifier);
				entity.Property(e => e.MusicBindingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.MusicIdentifier).IsRequired();
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired();
				entity.Property(e => e.Role).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => new { e.MusicIdentifier, e.MediaPersonalityIdentifier, e.Role }).IsUnique();
				entity.HasIndex(e => new { e.MusicIdentifier, e.MusicBindingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MediaPersonalityIdentifier, e.MusicBindingIdentifier }).IsUnique();
				entity.HasOne(e => e.Music)
					.WithMany(e => e.MusicBindings)
					.HasForeignKey(e => e.MusicIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MediaPersonality)
					.WithMany(e => e.MusicBindings)
					.HasForeignKey(e => e.MediaPersonalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}