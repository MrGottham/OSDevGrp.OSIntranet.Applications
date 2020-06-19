using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class CountryViewModel : AuditableViewModelBase
    {
        #region Properties

        [Display(Name = "Landekode", ShortName = "Kode", Description = "Landekode")]
        [Required(ErrorMessage = "Landekoden skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Længden på landekoden skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.CountryCodeRegexPattern, ErrorMessage = "Landekoden følger ikke det lovlige mønster: {1}")]
        public string Code { get; set; }

        [Display(Name = "Navn", ShortName = "Navn", Description = "Navn")]
        [Required(ErrorMessage = "Navnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på navnet skal være mellem {2} og {1} tegn.")]
        public string Name { get; set; }

        [Display(Name = "Universel navn", ShortName = "Universel", Description = "Universel navn")]
        [Required(ErrorMessage = "Universel navnet udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på universel navnet skal være mellem {2} og {1} tegn.")]
        public string UniversalName { get; set; }

        [Display(Name = "Landekode (telefon)", ShortName = "Kode (telefon)", Description = "Landekode (telefon)")]
        [Required(ErrorMessage = "Landekoden til telefon skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Længden på landekoden til telefon skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberPrefixRegexPattern, ErrorMessage = "Landekoden til telefon følger ikke det lovlige mønster: {1}")]
        public string PhonePrefix { get; set; }

        public bool Deletable { get; set; }

        public bool DefaultForPrincipal { get; set; }

        #endregion

        #region Methods

        public string GetDeletionLink(IUrlHelper urlHelper)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteCountry", "Contact");
        }

        public string GetDeletionData(IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"code: '{Code}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        public string GetPostalCodeCollectionLink(IUrlHelper urlHelper)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("PostalCodes", "Contact", new {countryCode = Code});
        }

        #endregion
    }
}