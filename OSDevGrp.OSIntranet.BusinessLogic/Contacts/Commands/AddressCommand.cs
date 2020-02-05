using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class AddressCommand : IAddressCommand
    {
        #region Properties

        public string StreetLine1 { get; set; }

        public string StreetLine2 { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.String.ShouldHaveMinLength(StreetLine1, 1, GetType(), nameof(StreetLine1), true)
                .String.ShouldHaveMinLength(StreetLine2, 1, GetType(), nameof(StreetLine2), true)
                .ValidateOptionalPostalCode(PostalCode, GetType(), nameof(PostalCode))
                .String.ShouldHaveMinLength(City, 1, GetType(), nameof(City), true)
                .String.ShouldHaveMinLength(State, 1, GetType(), nameof(State), true)
                .String.ShouldHaveMinLength(Country, 1, GetType(), nameof(Country), true);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(StreetLine1) &&
                   string.IsNullOrWhiteSpace(StreetLine2) &&
                   string.IsNullOrWhiteSpace(PostalCode) &&
                   string.IsNullOrWhiteSpace(City) &&
                   string.IsNullOrWhiteSpace(State) &&
                   string.IsNullOrWhiteSpace(Country);
        }

        public IAddress ToDomain()
        {
            if (IsEmpty())
            {
                return null;
            }

            return new Address
            {
                StreetLine1 = string.IsNullOrWhiteSpace(StreetLine1) ? null : StreetLine1,
                StreetLine2 = string.IsNullOrWhiteSpace(StreetLine2) ? null : StreetLine2,
                PostalCode = string.IsNullOrWhiteSpace(PostalCode) ? null : PostalCode,
                City = string.IsNullOrWhiteSpace(City) ? null : City,
                State = string.IsNullOrWhiteSpace(State) ? null : State,
                Country = string.IsNullOrWhiteSpace(Country) ? null : Country
            };
        }

        #endregion
    }
}