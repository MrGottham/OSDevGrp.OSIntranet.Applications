using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Mvc.Models.Security;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class SecurityController : Controller
    {
        #region Private variables

        private readonly IConverter _securityViewModelConverter = new SecurityViewModelConverter();

        #endregion

        #region Constructor

        public SecurityController()
        {
        }

        #endregion
    }
}