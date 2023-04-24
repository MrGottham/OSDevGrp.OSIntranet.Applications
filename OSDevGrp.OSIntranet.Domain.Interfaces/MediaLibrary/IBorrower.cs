using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IBorrower : IAuditable, IDeletable
	{
		Guid BorrowerIdentifier { get; }

		string ExternalIdentifier { get; }

		string FullName { get; }

		string MailAddress { get; }

		string PrimaryPhone { get; }

		string SecondaryPhone { get; }

		int LendingLimit { get; }

		IEnumerable<ILending> Lendings { get; }
	}
}