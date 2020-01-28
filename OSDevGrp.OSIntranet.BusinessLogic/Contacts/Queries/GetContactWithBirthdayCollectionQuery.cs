using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public class GetContactWithBirthdayCollectionQuery : ContactQueryBase, IGetContactWithBirthdayCollectionQuery
    {
        #region Properties

        public int BirthdayWithinDays { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .Integer.ShouldBeGreaterThanOrEqualToZero(BirthdayWithinDays, GetType(), nameof(BirthdayWithinDays));
        }

        #endregion
    }
}