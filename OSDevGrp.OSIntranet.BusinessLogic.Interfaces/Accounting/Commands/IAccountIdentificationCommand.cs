namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IAccountIdentificationCommand : IAccountingIdentificationCommand
    {
        string AccountNumber { get; set; }
    }
}