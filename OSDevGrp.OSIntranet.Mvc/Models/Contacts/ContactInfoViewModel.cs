﻿using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactInfoViewModel : ContactIdentificationViewModel
    {
        [Display(Name = "Mailadresse", ShortName = "Mail", Description = "Mailadresse")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "Længden på mailadressen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.MailAddressRegexPattern, ErrorMessage = "Mailadressen følger ikke det lovlige mønster: {1}")]
        public string MailAddress { get; set; }

        [Display(Name = "Primær telefonnummer", ShortName = "Primær telefonnr.", Description = "Primær telefonnummer")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på det primære telefonnummer skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det primære telefonummer følger ikke det lovlige mønster: {1}")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Sekundær telefonnummer", ShortName = "Sekundær telefonnr.", Description = "Sekundær telefonnummer")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på det sekundære telefonnummer skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det sekundære telefonummer følger ikke det lovlige mønster: {1}")]
        public string SecondaryPhone { get; set; }

        [Display(Name = "Telefonnummer (hjem)", ShortName = "Telefonnr. (hjem)", Description = "Telefonnummer (hjem)")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på telefonnummer (hjem) skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Telefonummer (hjem) følger ikke det lovlige mønster: {1}")]
        public string HomePhone
        {
            get => SecondaryPhone;
            set => SecondaryPhone = value;
        }

        [Display(Name = "Telefonnummer (mobil)", ShortName = "Telefonnr. (mobil)", Description = "Telefonnummer (mobil)")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på telefonnummer (mobil) skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Telefonummer (mobil) følger ikke det lovlige mønster: {1}")]
        public string MobilePhone
        {
            get => PrimaryPhone;
            set => PrimaryPhone = value;
        }

        public bool HasContactInfo => string.IsNullOrWhiteSpace(MailAddress) == false ||
                                      string.IsNullOrWhiteSpace(PrimaryPhone) == false ||
                                      string.IsNullOrWhiteSpace(SecondaryPhone) == false ||
                                      string.IsNullOrWhiteSpace(HomePhone) == false ||
                                      string.IsNullOrWhiteSpace(MobilePhone) == false;
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