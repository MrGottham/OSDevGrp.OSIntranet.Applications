using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public enum ContactType
    {
        [Display(Name =  "Ikke fastsat")]
        Unknown,

        [Display(Name = "Virksomhed")]
        Company,

        [Display(Name = "Person")]
        Person
    }

    public static class ContactTypeExtensions
    {
        public static ContactType ToContactType(this IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            if (contact.Name is ICompanyName)
            {
                return ContactType.Company;
            }

            if (contact.Name is IPersonName)
            {
                return ContactType.Person;
            }

            return ContactType.Unknown;
        }
    }
}