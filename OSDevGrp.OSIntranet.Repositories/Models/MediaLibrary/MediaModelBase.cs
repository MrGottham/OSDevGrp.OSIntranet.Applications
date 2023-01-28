using System;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal abstract class MediaModelBase : AuditModelBase
    {
        public virtual int MediaIdentifier { get; set; }

        public virtual Guid ExternalMediaIdentifier { get; set; }

        public virtual int CoreDataIdentifier { get; set; }

        public virtual MediaCoreDataModel CoreData { get; set; }

        public virtual bool Deletable { get; set; }
    }
}