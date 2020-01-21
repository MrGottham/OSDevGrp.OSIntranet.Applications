using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public abstract class ContactIdentificationQueryBase : ContactQueryBase, IContactIdentificationQuery
    {
        #region Properties

        public string ExternalIdentifier { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .ValidateExternalIdentifier(ExternalIdentifier, GetType(), nameof(ExternalIdentifier));
        }

        #endregion
    }
}