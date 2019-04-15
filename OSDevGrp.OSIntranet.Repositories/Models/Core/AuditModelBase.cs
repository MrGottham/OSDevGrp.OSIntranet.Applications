using System;

namespace OSDevGrp.OSIntranet.Repositories.Models.Core
{
    internal abstract class AuditModelBase
    {
        public virtual DateTime CreatedUtcDateTime { get; set; }

        public virtual string CreatedByIdentifier { get; set; }

        public virtual DateTime ModifiedUtcDateTime { get; set; }

        public virtual string ModifiedByIdentifier { get; set; }
    }
}