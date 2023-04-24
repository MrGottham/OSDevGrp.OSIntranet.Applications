using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface ILending : IAuditable, IDeletable
	{
		Guid LendingIdentifier { get; }

		IBorrower Borrower { get; }

		IMedia Media { get; }

		DateTime LendingDate { get; }

		bool Recall { get; }

		DateTime RecallDate { get; }

		bool Returned { get; }

		DateTime? ReturnedDate { get; }
	}
}