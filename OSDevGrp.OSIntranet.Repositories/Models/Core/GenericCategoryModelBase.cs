namespace OSDevGrp.OSIntranet.Repositories.Models.Core
{
    internal abstract class GenericCategoryModelBase : AuditModelBase
    {
        public string Name { get; set; }

        public virtual bool Deletable { get; set; }
    }
}