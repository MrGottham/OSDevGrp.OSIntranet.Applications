using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class PostalCodeViewModel : AuditableViewModelBase
    {
        #region Properties

        public CountryViewModel Country { get; set; }

        [Display(Name = "Postnummer", ShortName = "Postnr.", Description = "Postnummer")]
        [Required(ErrorMessage = "Postnummeret skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Længden på postnummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(@"[0-9]{1,16}", ErrorMessage = "Postnummeret følger ikke det lovlige mønster: {1}")]
        public string Code { get; set; }

        [Display(Name = "By", ShortName = "By", Description = "Bynavn")]
        [Required(ErrorMessage = "Bynavnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på bynavnet skal være mellem {2} og {1} tegn.")]
        public string City { get; set; }

        [Display(Name = "Stat", ShortName = "Stat", Description = "Stat")]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på staten skal være mellem {2} og {1} tegn.")]
        public string State { get; set; }

        public bool Deletable { get; set; }

        #endregion

        #region Methods

        public string GetDeletionLink(IUrlHelper urlHelper)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeletePostalCode", "Contact");
        }

        public string GetDeletionData(IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"countryCode: '{Country.Code}', postalCode: '{Code}', {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        #endregion
    }
}
