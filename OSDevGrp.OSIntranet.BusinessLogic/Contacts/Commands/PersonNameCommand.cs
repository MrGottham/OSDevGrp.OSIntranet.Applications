using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class PersonNameCommand : NameCommandBase, IPersonNameCommand
    {
        #region Properties

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.String.ShouldHaveMinLength(GivenName, 1, GetType(), nameof(GivenName), true)
                .String.ShouldHaveMinLength(MiddleName, 1, GetType(), nameof(MiddleName), true)
                .String.ShouldNotBeNullOrWhiteSpace(Surname, GetType(), nameof(Surname))
                .String.ShouldHaveMinLength(Surname, 1, GetType(), nameof(Surname));
        }

        public override IName ToDomain()
        {
            if (string.IsNullOrWhiteSpace(GivenName) && string.IsNullOrWhiteSpace(MiddleName))
            {
                return new PersonName(Surname);
            }

            if (string.IsNullOrWhiteSpace(MiddleName))
            {
                return new PersonName(GivenName, Surname);
            }

            return new PersonName(GivenName, MiddleName, Surname);
        }

        #endregion
    }
}