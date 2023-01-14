using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Core
{
    public abstract class GenericCategoryBase : AuditableBase, IGenericCategory
    {
        #region Constructor

        protected GenericCategoryBase(int number, string name, bool deletable = false)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            Number = number;
            Name = name;
            Deletable = deletable;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

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