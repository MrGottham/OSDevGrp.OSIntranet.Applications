using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;
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
    }
}