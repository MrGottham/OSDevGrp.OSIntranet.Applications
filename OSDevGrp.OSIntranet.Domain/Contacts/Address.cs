using System.Text;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Contacts
{
    public class Address : IAddress
    {
        #region Private variables

        private string _postalCode;
        private string _city;
        private string _state;
        private string _country;
        private readonly string[] _streetLines = new string[]
        {
            null,
            null
        };

        #endregion

        #region Properties

        public string StreetLine1
        {
            get => _streetLines[0];
            set => _streetLines[0] = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string StreetLine2
        {
            get => _streetLines[1];
            set => _streetLines[1] = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string PostalCode
        {
            get => _postalCode;
            set => _postalCode = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string City
        {
            get => _city;
            set => _city = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string State
        {
            get => _state;
            set => _state = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string Country
        {
            get => _country;
            set => _country = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public string DisplayAddress
        {
            get
            {
                StringBuilder resultBuilder = new StringBuilder();

                if (string.IsNullOrWhiteSpace(StreetLine1) == false)
                {
                    resultBuilder.AppendLine(StreetLine1);
                }

                if (string.IsNullOrWhiteSpace(StreetLine2) == false)
                {
                    resultBuilder.AppendLine(StreetLine2);
                }

                if (string.IsNullOrWhiteSpace(State) == false && string.IsNullOrWhiteSpace(PostalCode) == false && string.IsNullOrWhiteSpace(City) == false)
                {
                    resultBuilder.AppendLine($"{City}, {State} {PostalCode}");
                }
                else if (string.IsNullOrWhiteSpace(PostalCode) == false && string.IsNullOrWhiteSpace(City) == false)
                {
                    resultBuilder.AppendLine($"{PostalCode} {City}");
                }

                if (string.IsNullOrWhiteSpace(Country) == false)
                {
                    resultBuilder.AppendLine(Country);
                }

                return resultBuilder.ToString();
            }
        }

        #endregion
    }
}
