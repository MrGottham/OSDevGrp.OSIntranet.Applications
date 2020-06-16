using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountGroupViewModel : AccountGroupViewModelBase
    {
        #region Properties

        [Display(Name = "Type", ShortName = "Type", Description = "Typen for kontogruppen")]
        [Required(ErrorMessage = "Typen for kontogruppen skal vælges.")]
        public AccountGroupType AccountGroupType { get; set; }

        #endregion
 
        #region Methods

        public override string GetDeletionLink(IUrlHelper urlHelper)
        {
            NullGuard.NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("DeleteAccountGroup", "Accounting");
        }

        #endregion
    }
}