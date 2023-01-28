using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class MediaCoreDataModel : AuditModelBase
    {
        public virtual int MediaCoreDataIdentifier { get; set; }

        public virtual string Title { get; set; }

        public virtual string Subtitle { get; set; }

        public virtual string Description { get; set; }

        public virtual string Details { get; set; }

        public virtual int MediaTypeIdentifier { get; set; }

        public virtual MediaTypeModel MediaType { get; set; }

        public virtual short? Published { get; set; }

        public virtual string Url { get; set; }

        public virtual string Image { get; set; }
    }
}