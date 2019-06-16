namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IName
    {
        string DisplayName { get; }

        void SetName(string fullName);
    }
}
