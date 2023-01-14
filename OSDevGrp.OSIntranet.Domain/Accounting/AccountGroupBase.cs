using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountGroupBase : AuditableBase, IAccountGroupBase
    {
        #region Constructor

        protected AccountGroupBase(int number, string name, bool deletable)
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

        public bool IsProtected { get; private set; }

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

        public virtual void ApplyProtection()
        {
            DisallowDeletion();

            IsProtected = true;
        }

        public virtual void AllowDeletion()
        {
            Deletable = true;
        }

        public virtual void DisallowDeletion()
        {
            Deletable = false;
        }

        #endregion
    }
}