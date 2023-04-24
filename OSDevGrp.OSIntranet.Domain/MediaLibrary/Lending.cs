using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Globalization;
using System.Text;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class Lending : AuditableBase, ILending
	{
		#region Constructor

		public Lending(Guid lendingIdentifier, IBorrower borrower, IMedia media, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate, bool deletable = false)
		{
			NullGuard.NotNull(borrower, nameof(borrower))
				.NotNull(media, nameof(media));

			LendingIdentifier = lendingIdentifier;
			Borrower = borrower;
			Media = media;
			LendingDate = lendingDate.Date;
			RecallDate = recallDate.Date;
			ReturnedDate = returnedDate?.Date;
			Deletable = deletable;
		}

		#endregion

		#region Properties

		public Guid LendingIdentifier { get; }

		public IBorrower Borrower { get; }

		public IMedia Media { get; }

		public DateTime LendingDate { get; }

		public bool Recall => Returned == false && RecallDate.Date <= DateTime.Today;

		public DateTime RecallDate { get; }

		public bool Returned => ReturnedDate.HasValue;

		public DateTime? ReturnedDate { get; }

		public bool Deletable { get; private set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine($"Borrower: {Borrower}");
			builder.AppendLine($"Media: {Media}");
			builder.AppendLine($"Lending date: {LendingDate.ToString("D", CultureInfo.InvariantCulture)}");
			builder.Append($"Recall date: {RecallDate.ToString("D", CultureInfo.InvariantCulture)}");
			if (ReturnedDate.HasValue)
			{
				builder.AppendLine();
				builder.Append($"Returned: {ReturnedDate.Value.ToString("D", CultureInfo.InvariantCulture)}");
			}

			return builder.ToString();
		}

		public override bool Equals(object obj)
		{
			ILending lending = obj as ILending;
			return lending != null && LendingIdentifier.Equals(lending.LendingIdentifier);
		}

		public override int GetHashCode()
		{
			return LendingIdentifier.GetHashCode();
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