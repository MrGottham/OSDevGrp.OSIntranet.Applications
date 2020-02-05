using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands
{
    public class CompanyNameCommand : NameCommandBase, ICompanyNameCommand
    {
        #region Properties

        public string FullName { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.String.ShouldNotBeNullOrWhiteSpace(FullName, GetType(), nameof(FullName))
                .String.ShouldHaveMinLength(FullName, 1, GetType(), nameof(FullName));
        }

        public override IName ToDomain()
        {
            return new CompanyName(FullName);
        }

        #endregion
    }
}