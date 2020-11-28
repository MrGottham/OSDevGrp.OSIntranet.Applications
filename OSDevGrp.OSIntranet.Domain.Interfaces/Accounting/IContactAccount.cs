using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IContactAccount : IAccountBase<IContactAccount>
    {
        string MailAddress { get; set; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        IPaymentTerm PaymentTerm { get; set; }

        ContactAccountType ContactAccountType { get; }

        IContactInfoValues ValuesAtStatusDate { get; }

        IContactInfoValues ValuesAtEndOfLastMonthFromStatusDate { get; }

        IContactInfoValues ValuesAtEndOfLastYearFromStatusDate { get; }

        IContactInfoCollection ContactInfoCollection { get; }
    }
}