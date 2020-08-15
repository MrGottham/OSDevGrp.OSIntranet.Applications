using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactOptionsViewModel
    {
        #region Properties

        public string Filter { get; set; }

        [Display(Name = "Landekode", ShortName = "Kode", Description = "Landekode")]
        public string DefaultCountryCode { get; set; }

        public List<CountryViewModel> Countries { get; set; }

        #endregion
    }

    public static class ContactOptionsViewModelExtension
    {
        public static IEnumerable<SelectListItem> GetCountryItems(this ContactOptionsViewModel contactOptionsViewModel)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel));

            return contactOptionsViewModel.Countries?.Select(countryViewModel => new SelectListItem(countryViewModel.Name, countryViewModel.Code)).ToArray() ?? new SelectListItem[0];
        }

        public static string GetStartLoadingContactsUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return contactOptionsViewModel.GetStartLoadingContactsUrl(urlHelper, contactOptionsViewModel.Filter);
        }

        public static string GetStartLoadingContactsUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper, string filter)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return string.IsNullOrWhiteSpace(filter)
                ? urlHelper.AbsoluteAction("StartLoadingContacts", "Contact")
                : urlHelper.AbsoluteAction("StartLoadingContacts", "Contact", new {Filter = filter});
        }

        public static string GetLoadContactsUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return contactOptionsViewModel.GetLoadContactsUrl(urlHelper, contactOptionsViewModel.Filter);
        }

        public static string GetLoadContactsUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper, string filter)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return string.IsNullOrWhiteSpace(filter)
                ? urlHelper.AbsoluteAction("LoadContacts", "Contact")
                : urlHelper.AbsoluteAction("LoadContacts", "Contact", new {Filter = filter});
        }

        public static string GetStartCreatingContactUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartCreatingContact", "Contact", new {CountryCode = "{countryCode}" });
        }

        public static string GetCreateContactUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("CreateContact", "Contact", new {CountryCode = contactOptionsViewModel.DefaultCountryCode});
        }

        public static string GetAddAssociatedCompanyUrl(this ContactOptionsViewModel contactOptionsViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactOptionsViewModel, nameof(contactOptionsViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("AddAssociatedCompany", "Contact", new {CountryCode = contactOptionsViewModel.DefaultCountryCode});
        }
    }
}