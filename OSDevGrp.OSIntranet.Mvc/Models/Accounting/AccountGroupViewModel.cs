using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IEnumerable<SelectListItem> AccountGroupTypes => Enum.GetValues(typeof(AccountGroupType))
            .Cast<AccountGroupType>()
            .Select(accountGroupType => new SelectListItem(Convert.ToString(accountGroupType.EnumDisplayNameFor()), Convert.ToString(accountGroupType), accountGroupType == AccountGroupType))
            .ToList();

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