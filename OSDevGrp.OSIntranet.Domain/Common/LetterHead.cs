using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Common
{
    public class LetterHead : AuditableBase, ILetterHead
    {
        #region Private variables

        private string _companyIdentificationNumber;
        private readonly string[] _lines = new string[7]
        {
            null,
            null,
            null,
            null,
            null,
            null,
            null
        };

        #endregion

        #region Constructor

        public LetterHead(int number, string name, string line1)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(line1, nameof(line1));

            Number = number;
            Name = name;
            Line1 = line1;
        }

        #endregion

        #region Properties

        public int Number { get; }

        public string Name { get; }

        public string Line1 
        { 
            get => _lines[0];
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _lines[0] = value;
            }
        }

        public string Line2 
        { 
            get => _lines[1];
            set => _lines[1] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string Line3
        { 
            get => _lines[2];
            set => _lines[2] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string Line4
        { 
            get => _lines[3];
            set => _lines[3] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string Line5
        { 
            get => _lines[4];
            set => _lines[4] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string Line6
        { 
            get => _lines[5];
            set => _lines[5] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string Line7
        { 
            get => _lines[6];
            set => _lines[6] = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string CompanyIdentificationNumber 
        { 
            get => _companyIdentificationNumber;
            set => _companyIdentificationNumber = string.IsNullOrWhiteSpace(value) ? null : value;
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