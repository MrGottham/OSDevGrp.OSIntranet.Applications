using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public class GetMatchingContactCollectionQuery : ContactQueryBase, IGetMatchingContactCollectionQuery
    {
        #region Properties

        public string SearchFor { get; set; }

        public bool SearchWithinName { get; set; }

        public bool SearchWithinMailAddress { get; set; }

        public bool SearchWithinPrimaryPhone { get; set; }

        public bool SearchWithinSecondaryPhone { get; set; }

        public bool SearchWithinHomePhone { get; set; }

        public bool SearchWithinMobilePhone { get; set; }

        public SearchOptions SearchOptions => (SearchWithinName ? SearchOptions.Name : 0) |
                                              (SearchWithinMailAddress ? SearchOptions.MailAddress : 0) |
                                              (SearchWithinPrimaryPhone ? SearchOptions.PrimaryPhone : 0) |
                                              (SearchWithinSecondaryPhone ? SearchOptions.SecondaryPhone : 0) |
                                              (SearchWithinHomePhone ? SearchOptions.HomePhone : 0) |
                                              (SearchWithinMobilePhone ? SearchOptions.MobilePhone : 0);

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .String.ShouldNotBeNullOrWhiteSpace(SearchFor, GetType(), nameof(SearchFor))
                .Integer.ShouldBeGreaterThanZero((int) SearchOptions, GetType(), nameof(SearchOptions));
        }

        #endregion
    }
}