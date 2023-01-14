using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public abstract class MediaBase : AuditableBase, IMedia
    {
        #region Constructor

        protected MediaBase(Guid mediaIdentifier, string name, string description, byte[] image, bool deletable = false)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            MediaIdentifier = mediaIdentifier;
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Image = image ?? Array.Empty<byte>();
            Deletable = deletable;
        }

        #endregion

        #region Properties

        public Guid MediaIdentifier { get; }

        public string Name { get; }

        public string Description { get; }

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