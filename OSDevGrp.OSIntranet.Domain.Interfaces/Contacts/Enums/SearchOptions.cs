using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums
{
    [Flags]
    public enum SearchOptions
    {
        Name = 1,

        MailAddress = 2,

        PrimaryPhone = 4,

        SecondaryPhone = 8,

        HomePhone = 16,

        MobilePhone = 32
    }
}