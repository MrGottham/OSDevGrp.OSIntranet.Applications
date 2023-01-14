using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public abstract class MediaBase : AuditableBase, IMedia
    {
        #region Constructor

        protected MediaBase(Guid mediaIdentifier, string title, string subtitle, string description, IMediaType mediaType, short? published, string details, byte[] image, bool deletable = false)
        {
            NullGuard.NotNullOrWhiteSpace(title, nameof(title))
                .NotNull(mediaType, nameof(mediaType));

            MediaIdentifier = mediaIdentifier;
            Title = title.Trim();
            Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            MediaType = mediaType;
            Published = published;
            Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
            Image = image ?? Array.Empty<byte>();
            Deletable = deletable;
        }

        #endregion

        #region Properties

        public Guid MediaIdentifier { get; }

        public string Title { get; }

        public string Subtitle { get; }

        public string Description { get; }

        public IMediaType MediaType { get; }

        public string Details { get; }

        public short? Published { get; }

        public byte[] Image { get; }

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

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