using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class MediaPersonality : AuditableBase, IMediaPersonality
	{
		#region Constructor

		public MediaPersonality(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, INationality nationality, IEnumerable<MediaRole> roles, DateTime? birthDate, DateTime? dateOfDead, Uri url, byte[] image, bool deletable = false)
		{
			NullGuard.NotNull(surname, nameof(surname))
				.NotNull(nationality, nameof(nationality))
				.NotNull(roles, nameof(roles));

			MediaPersonalityIdentifier = mediaPersonalityIdentifier;
			GivenName = string.IsNullOrWhiteSpace(givenName) ? null : givenName.Trim();
			MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim();
			Surname = surname.Trim();
			Nationality = nationality;
			Roles = roles;
			BirthDate = birthDate;
			DateOfDead = dateOfDead;
			Url = url;
			Image = image ?? Array.Empty<byte>();
			Deletable = deletable;
		}

		#endregion

		#region Properties

		public Guid MediaPersonalityIdentifier { get; }

		public string GivenName { get; }

		public string MiddleName { get; }

		public string Surname { get; }

		public INationality Nationality { get; }

		public IEnumerable<MediaRole> Roles { get; }

		public DateTime? BirthDate { get; }

		public DateTime? DateOfDead { get; }

		public Uri Url { get; }

		public byte[] Image { get; }

		public bool Deletable { get; private set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			if (string.IsNullOrWhiteSpace(GivenName) == false && string.IsNullOrWhiteSpace(MiddleName) == false)
			{
				return $"{GivenName} {MiddleName} {Surname}";
			}

			if (string.IsNullOrWhiteSpace(GivenName) == false)
			{
				return $"{GivenName} {Surname}";
			}

			if (string.IsNullOrWhiteSpace(MiddleName) == false)
			{
				return $"{MiddleName} {Surname}";
			}

			return Surname;
		}

		public override bool Equals(object obj)
		{
			IMediaPersonality mediaPersonality = obj as IMediaPersonality;
			return mediaPersonality != null && MediaPersonalityIdentifier.Equals(mediaPersonality.MediaPersonalityIdentifier);
		}

		public override int GetHashCode()
		{
			return MediaPersonalityIdentifier.GetHashCode();
		}

		public void AllowDeletion()
		{
			Deletable = true;
		}

		public void DisallowDeletion()
		{
			Deletable = false;
		}

		#endregion
	}
}