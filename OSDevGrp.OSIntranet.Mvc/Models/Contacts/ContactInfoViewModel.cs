using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactInfoViewModel : ContactIdentificationViewModel
    {
        public string MailAddress { get; set; }

        public string HomePhone { get; set; }

        public string MobilePhone { get; set; }
    }

    public static class ContactInfoViewModelExtensions
    {
        public static string DisplayContactInfo(this ContactInfoViewModel contactInfoViewModel)
        {
            NullGuard.NotNull(contactInfoViewModel, nameof(contactInfoViewModel));

            if (string.IsNullOrWhiteSpace(contactInfoViewModel.MailAddress) == false)
            {
                return contactInfoViewModel.MailAddress;
            }

            if (contactInfoViewModel.ContactType == ContactType.Person)
            {
                if (string.IsNullOrWhiteSpace(contactInfoViewModel.MobilePhone) == false)
                {
                    return contactInfoViewModel.MobilePhone;
                }

                return string.IsNullOrWhiteSpace(contactInfoViewModel.HomePhone) ? null : contactInfoViewModel.HomePhone;
            }

            if (string.IsNullOrWhiteSpace(contactInfoViewModel.HomePhone) == false)
            {
                return contactInfoViewModel.HomePhone;
            }

            return string.IsNullOrWhiteSpace(contactInfoViewModel.MobilePhone) ? null : contactInfoViewModel.MobilePhone;
        }
    }
}