using System;
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

        string MailAddress { get; set; }

        ICompany Company { get; set; }
    }
}
