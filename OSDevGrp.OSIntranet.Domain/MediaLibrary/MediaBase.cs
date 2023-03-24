using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public abstract class MediaBase : AuditableBase, IMedia
    {
        #region Constructor

        protected MediaBase(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMediaType mediaType, short? published, Uri url, byte[] image, IEnumerable<IMediaBinding> mediaBindings, bool deletable = false)
        {
	        NullGuard.NotNullOrWhiteSpace(title, nameof(title))
		        .NotNull(mediaType, nameof(mediaType))
		        .NotNull(mediaBindings, nameof(mediaBindings));

            MediaIdentifier = mediaIdentifier;
            Title = title.Trim();
            Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
            MediaType = mediaType;
            Published = published;
            Url = url;
            Image = image ?? Array.Empty<byte>();
            Deletable = deletable;
            MediaBindings = new HashSet<IMediaBinding>(mediaBindings);
        }

        #endregion

        #region Properties

        public Guid MediaIdentifier { get; }

        public string Title { get; }

        public string Subtitle { get; }

        public string Description { get; }

        public string Details { get; }

        public IMediaType MediaType { get; }

        public short? Published { get; }

        public Uri Url { get; }

        public byte[] Image { get; }

        public bool Deletable { get; private set; }

        protected HashSet<IMediaBinding> MediaBindings { get; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return string.IsNullOrWhiteSpace(Subtitle) == false
				? $"{Title}, {Subtitle}"
				: Title;
		}

		public override bool Equals(object obj)
		{
			IMedia media = obj as IMedia;
			return media != null && MediaIdentifier.Equals(media.MediaIdentifier);
		}

		public override int GetHashCode()
		{
			return MediaIdentifier.GetHashCode();
		}

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public IEnumerable<IMediaBinding> GetMediaBindings() => MediaBindings;

        #endregion
    }
}