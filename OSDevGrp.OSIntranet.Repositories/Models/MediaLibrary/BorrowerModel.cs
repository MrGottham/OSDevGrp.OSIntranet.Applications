using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class BorrowerModel : AuditModelBase
	{
		public virtual int BorrowerIdentifier { get; set; }

		public virtual Guid ExternalBorrowerIdentifier { get; set; }

		public virtual string FullName { get; set; }

		public virtual string MailAddress { get; set; }

		public virtual string PrimaryPhone { get; set; }

		public virtual string SecondaryPhone { get; set; }

		public virtual int LendingLimit { get; set; }

		public virtual bool Deletable { get; set; }

		public virtual List<LendingModel> Lendings { get; set; }
	}

	internal static class BorrowerModelExtensions
	{
		#region Methods

		internal static IBorrower ToDomain(this BorrowerModel borrowerModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(borrowerModel, nameof(borrowerModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			lock (mapperCache.SyncRoot)
			{
				if (mapperCache.BorrowerDictionary.TryGetValue(borrowerModel.ExternalBorrowerIdentifier, out IBorrower cachedBorrower))
				{
					return cachedBorrower;
				}

				IBorrower borrower = new Borrower(
					borrowerModel.ExternalBorrowerIdentifier,
					borrowerModel.FullName,
					borrowerModel.MailAddress,
					borrowerModel.PrimaryPhone,
					borrowerModel.SecondaryPhone,
					borrowerModel.LendingLimit,
					borrower => (borrowerModel.Lendings ?? new List<LendingModel>(0)).ToDomain(borrower, mapperCache, mediaLibraryModelConverter, commonModelConverter));

				borrower.SetDeletable(borrowerModel.Deletable);

				DateTime modifiedUtcDateTime = borrowerModel.ModifiedUtcDateTime;
				string modifiedByIdentifier = borrowerModel.ModifiedByIdentifier;
				LendingModel latestModifiedLendingModel = (borrowerModel.Lendings ?? new List<LendingModel>(0)).GetLatestModifiedLendingModel();
				if (latestModifiedLendingModel != null && latestModifiedLendingModel.ModifiedUtcDateTime > modifiedUtcDateTime)
				{
					modifiedUtcDateTime = latestModifiedLendingModel.ModifiedUtcDateTime;
					modifiedByIdentifier = latestModifiedLendingModel.ModifiedByIdentifier;
				}
				borrower.AddAuditInformation(borrowerModel.CreatedUtcDateTime, borrowerModel.CreatedByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);

				mapperCache.Cache(borrower);

				return borrower;
			}
		}

		internal static void CreateBorrowerModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<BorrowerModel>(entity =>
			{
				entity.HasKey(e => e.BorrowerIdentifier);
				entity.Property(e => e.BorrowerIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalBorrowerIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.FullName).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.MailAddress).IsRequired(false).IsUnicode().HasMaxLength(256);
				entity.Property(e => e.PrimaryPhone).IsRequired(false).IsUnicode().HasMaxLength(32);
				entity.Property(e => e.SecondaryPhone).IsRequired(false).IsUnicode().HasMaxLength(32);
				entity.Property(e => e.LendingLimit).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalBorrowerIdentifier).IsUnique();
				entity.HasIndex(e => new { e.FullName, e.BorrowerIdentifier }).IsUnique();
			});
		}

		#endregion
	}
}