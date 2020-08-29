using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts
{
    public interface IContact : IAuditable
    {
        string InternalIdentifier { get; set; }

        string ExternalIdentifier { get; set; }

        IName Name { get; }

        IAddress Address { get; }

        string PrimaryPhone { get; set; }

        string SecondaryPhone { get; set; }

        string HomePhone { get; set; }

        string MobilePhone { get; set; }

        DateTime? Birthday { get; set; }

        ushort? Age { get; }

        string MailAddress { get; set; }

        ICompany Company { get; set; }

        IContactGroup ContactGroup { get; set; }

        string Acquaintance { get; set; }

        string PersonalHomePage { get; set; }

        int LendingLimit { get; set; }

        IPaymentTerm PaymentTerm { get; set; }

        bool IsMatch(string searchFor, SearchOptions searchOptions);

        bool HasBirthdayWithinDays(int days);

        string CalculateIdentifier();
    }
}