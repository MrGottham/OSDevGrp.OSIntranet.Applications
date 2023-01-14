using OSDevGrp.OSIntranet.Domain.Core;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal abstract class MediaModelBase : AuditableBase
    {
        public virtual Guid MediaIdentifier { get; set; }

        public virtual int CoreDataIdentifier { get; set; }

        public virtual MediaCoreDataModel CoreData { get; set; }

        public virtual bool Deletable { get; set; }
    }
}