using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class MediaBinding : AuditableBase, IMediaBinding
	{
		#region Constructor

		public MediaBinding(IMedia media, MediaRole role, IMediaPersonality mediaPersonality, bool deletable = false)
		{
			NullGuard.NotNull(media, nameof(media))
				.NotNull(mediaPersonality, nameof(mediaPersonality));

			Media = media;
			Role = role;
			MediaPersonality = mediaPersonality;
			Deletable = deletable;
		}

		#endregion

		#region Properties

		public IMedia Media { get; }

		public MediaRole Role { get; }

		public IMediaPersonality MediaPersonality { get; }

		public bool Deletable { get; private set; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return $"{Role}: {MediaPersonality}";
		}

		public override bool Equals(object obj)
		{
			IMediaBinding mediaBinding = obj as IMediaBinding;
			if (mediaBinding == null)
			{
				return false;
			}

			return string.Compare(this.CalculateKey(), mediaBinding.CalculateKey(), StringComparison.InvariantCulture) == 0;
		}

		public override int GetHashCode()
		{
			return this.CalculateKey().GetHashCode();
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