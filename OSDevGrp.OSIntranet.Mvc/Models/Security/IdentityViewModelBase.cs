using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Security
{
    public abstract class IdentityViewModelBase : AuditableViewModelBase
    {
        #region Properties

        public int Identifier { get; set; }

        public List<ClaimViewModel> Claims { get; set; }

        #endregion

        #region Methods

        public abstract string GetDeletionLink(IUrlHelper urlHelper);

        public string GetDeletionData(IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            return '{' + $"identifier: {Identifier}, {htmlHelper.AntiForgeryTokenToJsonString()}" + '}';
        }

        #endregion
    }
}
