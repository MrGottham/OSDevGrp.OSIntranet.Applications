using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class MediaPersonalityDataCommandBase : MediaPersonalityIdentificationCommandBase, IMediaPersonalityDataCommand
	{
		#region Constructor

		public MediaPersonalityDataCommandBase(Guid mediaIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image)
		: base(mediaIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(surname, nameof(surname));

			GivenName = string.IsNullOrWhiteSpace(givenName) ? null : givenName.Trim();
			MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim();
			Surname = surname.Trim();
			NationalityIdentifier = nationalityIdentifier;
			BirthDate = birthDate;
			DateOfDead = dateOfDead;
			Url = string.IsNullOrWhiteSpace(url) ? null : url.Trim();
			Image = image ?? Array.Empty<byte>();
		}

		#endregion

		#region Properties

		public string GivenName { get; }

		public string MiddleName { get; }

		public string Surname { get; }

		public int NationalityIdentifier { get; }

		public DateTime? BirthDate { get; }

		public DateTime? DateOfDead { get; }

		public string Url { get; }

		public byte[] Image { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateMediaPersonalityData(this, mediaLibraryRepository, commonRepository);
		}

		public async Task<IMediaPersonality> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			INationality nationality = await commonRepository.GetNationalityAsync(NationalityIdentifier);

			return new MediaPersonality(
				MediaPersonalityIdentifier,
				GivenName,
				MiddleName,
				Surname,
				nationality,
				Array.Empty<MediaRole>(),
				BirthDate,
				DateOfDead,
				string.IsNullOrWhiteSpace(Url) == false ? new Uri(Url, UriKind.Absolute) : null,
				Image);
		}

		#endregion
	}
}