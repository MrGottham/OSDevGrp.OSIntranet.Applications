using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class Borrower : AuditableBase, IBorrower
	{
		#region Constructor

		public Borrower(Guid borrowerIdentifier, string externalIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit, Func<IBorrower, IEnumerable<ILending>> lendingsBuilder, bool deletable = false)
		{
			NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName))
				.NotNull(lendingsBuilder, nameof(lendingsBuilder));

			BorrowerIdentifier = borrowerIdentifier;
			ExternalIdentifier = string.IsNullOrWhiteSpace(externalIdentifier) == false ? externalIdentifier.Trim() : null;
			FullName = fullName.Trim();
			MailAddress = string.IsNullOrWhiteSpace(mailAddress) == false ? mailAddress.Trim() : null;
			PrimaryPhone = string.IsNullOrWhiteSpace(primaryPhone) == false ? primaryPhone.Trim() : null;
			SecondaryPhone = string.IsNullOrWhiteSpace(secondaryPhone) == false ? secondaryPhone.Trim() : null;
			LendingLimit = lendingLimit;
			Lendings = new HashSet<ILending>(lendingsBuilder(this));
			Deletable = deletable;
		}

		#endregion

		#region Properties

		public Guid BorrowerIdentifier { get; }

		public string ExternalIdentifier { get; }

		public string FullName { get; }

		public string MailAddress { get; }

		public string PrimaryPhone { get; }

		public string SecondaryPhone { get; }

		public int LendingLimit { get; }

		public IEnumerable<ILending> Lendings { get; }

		public bool Deletable { get; private set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			if (string.IsNullOrWhiteSpace(MailAddress) && string.IsNullOrWhiteSpace(PrimaryPhone) && string.IsNullOrWhiteSpace(SecondaryPhone))
			{
				return FullName;
			}

			if (string.IsNullOrWhiteSpace(MailAddress) == false)
			{
				return $"{FullName} ({MailAddress})";
			}

			return string.IsNullOrWhiteSpace(PrimaryPhone) == false
				? $"{FullName} ({PrimaryPhone})"
				: $"{FullName} ({SecondaryPhone})";
		}

		public override bool Equals(object obj)
		{
			IBorrower borrower = obj as IBorrower;
			return borrower != null && BorrowerIdentifier.Equals(borrower.BorrowerIdentifier);
		}

		public override int GetHashCode()
		{
			return BorrowerIdentifier.GetHashCode();
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