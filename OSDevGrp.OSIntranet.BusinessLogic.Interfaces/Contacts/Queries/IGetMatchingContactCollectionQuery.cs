using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IGetMatchingContactCollectionQuery : IContactQuery
    {
        string SearchFor { get; set; }

        bool SearchWithinName { get; set; }

        bool SearchWithinMailAddress { get; set; }

        bool SearchWithinPrimaryPhone { get; set; }

        bool SearchWithinSecondaryPhone { get; set; }

        bool SearchWithinHomePhone { get; set; }

        bool SearchWithinMobilePhone { get; set; }

        SearchOptions SearchOptions { get; }
    }
}