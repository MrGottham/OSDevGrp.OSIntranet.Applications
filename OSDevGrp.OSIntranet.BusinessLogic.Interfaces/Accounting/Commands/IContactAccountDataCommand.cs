using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands
{
    public interface IContactAccountDataCommand : IAccountCoreDataCommand<IContactAccount>
    {
        string MailAddress { get; set; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        int PaymentTermNumber { get; set; }
    }
}