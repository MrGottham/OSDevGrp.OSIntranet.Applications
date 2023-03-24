using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IMediaPersonality : IAuditable, IDeletable
	{
		Guid MediaPersonalityIdentifier { get; }

		string GivenName { get; }

		string MiddleName { get; }

		string Surname { get; }

		INationality Nationality { get; }

		IEnumerable<MediaRole> Roles { get; }

		DateTime? BirthDate { get; }

		DateTime? DateOfDead { get; }

		Uri Url { get; }

		byte[] Image { get; }
	}
}