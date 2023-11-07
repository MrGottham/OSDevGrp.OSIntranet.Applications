using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class LendingDataCommandBase : LendingIdentificationCommandBase, ILendingDataCommand
	{
		#region Constructor

		protected LendingDataCommandBase(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate) 
			: base(lendingIdentifier)
		{
			BorrowerIdentifier = borrowerIdentifier;
			MediaIdentifier = mediaIdentifier;
			LendingDate = lendingDate.Date;
			RecallDate = recallDate.Date;
			ReturnedDate = returnedDate?.Date;
		}

		#endregion

		#region Properties

		public Guid BorrowerIdentifier { get; }

		public Guid MediaIdentifier { get; }

		public DateTime LendingDate { get; }

		public DateTime RecallDate { get; }

		public DateTime? ReturnedDate { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateLendingData(this, mediaLibraryRepository, commonRepository);
		}

		public async Task<ILending> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			IBorrower borrower = await mediaLibraryRepository.GetBorrowerAsync(BorrowerIdentifier);
			IMedia media = await MediaResolver.ResolveAsync(MediaIdentifier, mediaLibraryRepository);

			return new Lending(
				LendingIdentifier, 
				borrower,
				media,
				LendingDate,
				RecallDate,
				ReturnedDate);
		}

		#endregion
	}
}