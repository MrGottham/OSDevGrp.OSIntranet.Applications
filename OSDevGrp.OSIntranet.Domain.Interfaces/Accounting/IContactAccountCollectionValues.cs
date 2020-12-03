namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactAccountCollectionValues
    {
        decimal Debtors { get; }

        decimal Creditors { get; }
    }
}