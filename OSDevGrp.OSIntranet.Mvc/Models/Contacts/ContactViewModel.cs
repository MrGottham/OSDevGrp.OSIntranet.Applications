using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactViewModel : ContactInfoViewModel
    {
        [Display(Name = "Fornavn", ShortName = "Fornavn", Description = "Personens fornavn")]
        public string GivenName { get; set; }

        [Display(Name = "Mellemnavn(e)", ShortName = "Mellemnavn(e)", Description = "Personens mellemnavn(e)")]
        public string MiddleName { get; set; }

        [Display(Name = "Efternavn", ShortName = "Efternavn", Description = "Personens efternavn")]
        [Required(ErrorMessage = "Efternavnet skal udfyldes.", AllowEmptyStrings = false)]
        public string Surname { get; set; }

        [Display(Name = "Firmanavn", ShortName = "Firmanavn", Description = "Firmaets navn")]
        [Required(ErrorMessage = "Firmanavn skal udfyldes.", AllowEmptyStrings = false)]
        public string CompanyName
        {
            get => Surname;
            set => Surname = value;
        }

        [Display(Name = "Adresse", ShortName = "Adresse", Description = "Adresseoplysninger")]
        [Required(ErrorMessage = "Adressen skal angives.")]
        public AddressViewModel Address { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Fødselsdato", ShortName = "Fødselsdato", Description = "Fødselsdato")]
        [DisplayFormat(DataFormatString = "{0:d. MMMM yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Kontaktgruppe", ShortName = "Kontaktgruppe", Description = "Kontaktgruppe")]
        [Required(ErrorMessage = "Kontaktgruppen skal vælges.")]
        public ContactGroupViewModel ContactGroup { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Bekendtskab", ShortName = "Bekendtskab", Description = "Bekendtskab")]
        [StringLength(4096, MinimumLength = 0, ErrorMessage = "Længden på bekendtskabet skal være mellem {2} og {1} tegn.")]
        public string Acquaintance { get; set; }

        [Display(Name = "Webside", ShortName = "Webside", Description = "Webside")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "Længden på websiden skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.UrlRegexPattern, ErrorMessage = "Websiden følger ikke det lovlige mønster: {1}")]
        public string HomePage { get; set; }

        [Display(Name = "Udlånsfrist", ShortName = "Udlånsfrist", Description = "Udlånsfrist i dage")]
        [Required(ErrorMessage = "Udlånsfristen skal udfyldes-")]
        [Range(1, 365, ErrorMessage = "Udlånsfristen skal være mellem {1} og {2}.")]
        public int LendingLimit { get; set; }

        [Display(Name = "Betalingsbetingelse", ShortName = "Betalingsbetingelse", Description = "Betalingsbetingelse")]
        [Required(ErrorMessage = "Betalingsbetingelsen skal vælges.")]
        public PaymentTermViewModel PaymentTerm { get; set; }

        public CompanyViewModel Company { get; set; }

        public CountryViewModel Country { get; set; }

        public List<CountryViewModel> Countries { get; set; }

        public List<ContactGroupViewModel> ContactGroups { get; set; }

        public List<PaymentTermViewModel> PaymentTerms { get; set; }
    }

    public static class ContactViewModelExtensions
    {
        public static string GetAction(this ContactViewModel contactViewModel)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel));

            return contactViewModel.EditMode == EditMode.Create ? "CreateContact" : "UpdateContact";
        }

        public static string GetActionText(this ContactViewModel contactViewModel)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel));

            return contactViewModel.EditMode == EditMode.Create ? "Opret" : "Opdatér";
        }

        public static string GetDeletionLink(this ContactViewModel contactViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteContact", "Contact");
        }

        public static string GetDeletionData(this ContactViewModel contactViewModel, IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel))
                .NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"externalIdentifier: '{contactViewModel.ExternalIdentifier}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public static string GetStartAddingAssociatedCompanyUrl(this ContactViewModel contactViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactViewModel, nameof(contactViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartAddingAssociatedCompany", "Contact", new {countryCode = "{countryCode}"});
        }
    }
}