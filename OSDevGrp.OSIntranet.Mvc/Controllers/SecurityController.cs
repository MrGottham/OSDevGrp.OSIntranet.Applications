using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Security;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = "Accounting")]
    public class SecurityController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IConverter _securityViewModelConverter = new SecurityViewModelConverter();

        #endregion

        #region Constructor

        public SecurityController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
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
        public async Task<IActionResult> CreateUserIdentity()
        {
            IEnumerable<Claim> systemClaims = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<Claim>>(new EmptyQuery());

            UserIdentityViewModel userIdentityViewModel = new UserIdentityViewModel
            {
                Claims = BuildClaimViewModelCollection(systemClaims),
                EditMode = EditMode.Create
            };

            return View("CreateUserIdentity", userIdentityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserIdentity(UserIdentityViewModel userIdentityViewModel)
        {
            NullGuard.NotNull(userIdentityViewModel, nameof(userIdentityViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateUserIdentity", userIdentityViewModel);
            }

            ICreateUserIdentityCommand command = _securityViewModelConverter.Convert<UserIdentityViewModel, CreateUserIdentityCommand>(userIdentityViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("UserIdentities", "Security");
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

        [HttpGet]
        public async Task<IActionResult> CreateClientSecretIdentity()
        {
            IEnumerable<Claim> systemClaims = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<Claim>>(new EmptyQuery());

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = new ClientSecretIdentityViewModel
            {
                Claims = BuildClaimViewModelCollection(systemClaims),
                EditMode = EditMode.Create
            };

            return View("CreateClientSecretIdentity", clientSecretIdentityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientSecretIdentity(ClientSecretIdentityViewModel clientSecretIdentityViewModel)
        {
            NullGuard.NotNull(clientSecretIdentityViewModel, nameof(clientSecretIdentityViewModel));

            if (ModelState.IsValid == false)
            {
                return View("CreateClientSecretIdentity", clientSecretIdentityViewModel);
            }

            ICreateClientSecretIdentityCommand command = _securityViewModelConverter.Convert<ClientSecretIdentityViewModel, CreateClientSecretIdentityCommand>(clientSecretIdentityViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("ClientSecretIdentities", "Security");
        }

        private List<ClaimViewModel> BuildClaimViewModelCollection(IEnumerable<Claim> systemClaims)
        {
            NullGuard.NotNull(systemClaims, nameof(systemClaims));

            return _securityViewModelConverter.Convert<IEnumerable<Claim>, IEnumerable<ClaimViewModel>>(systemClaims).OrderBy(m => m.ClaimType).ToList();
        }

        #endregion
    }
}