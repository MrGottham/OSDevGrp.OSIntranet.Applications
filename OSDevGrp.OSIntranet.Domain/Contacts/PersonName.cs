using System.Text;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class PersonName : NameBase, IPersonName
    {
        #region Private variables

        private string _givenName;
        private string _middleName;
        private string _surname;
        private readonly Regex _fullNameRegex = new Regex(@"(.[^\s]+[\s]+)?(.+[\s]+)*(.[^\s]+){1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Constructors

        public PersonName(string surname)
        {
            NullGuard.NotNullOrWhiteSpace(surname, nameof(surname));

            Surname = surname;
        }

        public PersonName(string givenName, string surname)
        {
            NullGuard.NotNullOrWhiteSpace(givenName, nameof(givenName))
                .NotNullOrWhiteSpace(surname, nameof(surname));

            GivenName = givenName;
            Surname = surname;
        }

        public PersonName(string givenName, string middleName, string surname)
        {
            NullGuard.NotNullOrWhiteSpace(givenName, nameof(givenName))
                .NotNullOrWhiteSpace(middleName, nameof(middleName))
                .NotNullOrWhiteSpace(surname, nameof(surname));

            GivenName = givenName;
            MiddleName = middleName;
            Surname = surname;
        }

        #endregion

        #region Properties

        public string GivenName
        {
            get => _givenName;
            set => _givenName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string MiddleName
        {
            get => _middleName;
            set => _middleName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string Surname
        {
            get => _surname;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _surname = value.Trim();
            }
        }

        public override string DisplayName
        {
            get
            {
                StringBuilder displayNameBuilder = new StringBuilder();
                
                if (string.IsNullOrWhiteSpace(GivenName) == false)
                {
                    displayNameBuilder.Append(GivenName);
                }

                if (string.IsNullOrWhiteSpace(MiddleName) == false)
                {
                    if (displayNameBuilder.Length > 0)
                    {
                        displayNameBuilder.Append(" ");
                    }
                    displayNameBuilder.Append(MiddleName);
                }

                if (displayNameBuilder.Length > 0)
                {
                    displayNameBuilder.Append(" ");
                }
                displayNameBuilder.Append(Surname);

                return displayNameBuilder.ToString();
            }
        }

        #endregion

        #region Methods

        public override void SetName(string fullName)
        {
            NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName));

            Match match = _fullNameRegex.Match(fullName);
            if (match.Success == false)
            {
                GivenName = null;
                MiddleName = null;
                Surname = fullName;
                return;
            }

            if (string.IsNullOrWhiteSpace(match.Groups[1].Value) && string.IsNullOrWhiteSpace(match.Groups[2].Value))
            {
                GivenName = null;
                MiddleName = null;
                Surname = match.Groups[3].Value;
                return;
            }

            if (string.IsNullOrWhiteSpace(match.Groups[2].Value))
            {
                GivenName = match.Groups[1].Value;
                MiddleName = null;
                Surname = match.Groups[3].Value;
                return;
            }

            GivenName = match.Groups[1].Value;
            MiddleName = match.Groups[2].Value;
            Surname = match.Groups[3].Value;
        }

        #endregion
    }
}
