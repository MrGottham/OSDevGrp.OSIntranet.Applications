using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class MediaCoreDataModel : AuditModelBase
    {
        public virtual int MediaCoreDataIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Image { get; set; }
    }
}