using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Security;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class SecurityController : Controller
    {
        #region Private variables

        private readonly IQueryBus _queryBus;
        private readonly IConverter _securityViewModelConverter = new SecurityViewModelConverter();

        #endregion

        #region Constructor

        public SecurityController(IQueryBus queryBus)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus));

            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> UserIdentities()
        {
            IEnumerable<IUserIdentity> userIdentities = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IUserIdentity>>(new EmptyQuery());

            IEnumerable<UserIdentityViewModel> userIdentityViewModels = userIdentities.AsParallel()
                .Select(userIdentity => _securityViewModelConverter.Convert<IUserIdentity, UserIdentityViewModel>(userIdentity))
                .OrderBy(userIdentityViewModel => userIdentityViewModel.ExternalUserIdentifier)
                .ToList();

            return View("UserIdentities", userIdentityViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> ClientSecretIdentities()
        {
            IEnumerable<IClientSecretIdentity> clientSecretIdentities = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IClientSecretIdentity>>(new EmptyQuery());

            IEnumerable<ClientSecretIdentityViewModel> clientSecretIdentityViewModels = clientSecretIdentities.AsParallel()
                .Select(clientSecretIdentity => _securityViewModelConverter.Convert<IClientSecretIdentity, ClientSecretIdentityViewModel>(clientSecretIdentity))
                .OrderBy(clientSecretIdentityViewModel => clientSecretIdentityViewModel.FriendlyName)
                .ToList();

            return View("ClientSecretIdentities", clientSecretIdentityViewModels);
        }

        #endregion
    }
}