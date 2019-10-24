using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class ContactGroup : AuditableBase, IContactGroup
    {
        #region Private variables

        private string _name;

        #endregion

        #region Constructor

        public ContactGroup(int number, string name)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name));

            Number = number;
            Name = name;
        }

        #endregion

        #region Properties

        public int Number { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _name = value.Trim();
            }
        }

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
