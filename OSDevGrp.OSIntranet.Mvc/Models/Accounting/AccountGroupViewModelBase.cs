using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class AccountGroupViewModelBase : AuditableViewModelBase
    {
        #region Properties

        [Display(Name = "Nummer", ShortName = "Nummer", Description = "Nummer")]
        [Required(ErrorMessage = "Nummeret skal udfyldes.")]
        [Range(1, 99, ErrorMessage = "Nummeret skal være mellem {1} og {2}.")]
        public int Number { get; set; }

        [Display(Name = "Navn", ShortName = "Navn", Description = "Navn")]
        [Required(ErrorMessage = "Navnet skal udfyldes.", AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 1, ErrorMessage = "Længden på navnet skal være mellem {2} og {1} tegn.")]
        public string Name { get; set; }

        public bool Deletable { get; set; }

        #endregion

        #region Methods

        public abstract string GetDeletionLink(IUrlHelper urlHelper);

        public string GetDeletionData(IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"number: {Number}, {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        #endregion
    }
}