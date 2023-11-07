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
	internal abstract class BorrowerDataCommandBase : BorrowerIdentificationCommandBase, IBorrowerDataCommand
	{
		#region Constructor

		protected BorrowerDataCommandBase(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit) 
			: base(borrowerIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName));

			FullName = fullName.Trim();
			MailAddress = string.IsNullOrWhiteSpace(mailAddress) == false ? mailAddress.Trim() : null;
			PrimaryPhone = string.IsNullOrWhiteSpace(primaryPhone) == false ? primaryPhone.Trim() : null;
			SecondaryPhone = string.IsNullOrWhiteSpace(secondaryPhone) == false ? secondaryPhone.Trim() : null;
			LendingLimit = lendingLimit;
		}

		#endregion

		#region Properties

		public string FullName { get; }

		public string MailAddress { get; }

		public string PrimaryPhone { get; }

		public string SecondaryPhone { get; }

		public int LendingLimit { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateBorrowerData(this, mediaLibraryRepository, commonRepository);
		}

		public Task<IBorrower> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return Task.FromResult<IBorrower>(new Borrower(
				BorrowerIdentifier,
				FullName,
				MailAddress,
				PrimaryPhone,
				SecondaryPhone,
				LendingLimit,
				_ => Array.Empty<ILending>()));
		}

		#endregion
	}
}