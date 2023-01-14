using OSDevGrp.OSIntranet.Domain.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Core
{
    internal abstract class GenericCategoryModelBase : AuditableBase
    {
        public abstract int GenericCategoryIdentifier { get; }

        public string Name { get; set; }

        public virtual bool Deletable { get; set; }
    }
}