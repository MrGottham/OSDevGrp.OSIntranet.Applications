namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries
{
    public interface IGetContactWithBirthdayCollectionQuery : IContactQuery
    {
        int BirthdayWithinDays { get; set; }
    }
}