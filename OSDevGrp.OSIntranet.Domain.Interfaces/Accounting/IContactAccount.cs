namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactAccount : IAccountBase<IContactAccount>
    {
        string MailAddress { get; set; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        IPaymentTerm PaymentTerm { get; set; }
    }
}