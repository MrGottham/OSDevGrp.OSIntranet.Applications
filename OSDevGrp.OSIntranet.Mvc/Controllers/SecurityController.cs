using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Queries;
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
                Claims = BuildClaimViewModelCollection(systemClaims, new List<Claim>(0)),
                EditMode = EditMode.Create
            };

            return View("CreateUserIdentity", userIdentityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> UpdateUserIdentity(int identifier)
        {
            Task<IEnumerable<Claim>> systemClaimsTask = _queryBus.QueryAsync<EmptyQuery, IEnumerable<Claim>>(new EmptyQuery());
            Task<IUserIdentity> userIdentityTask = _queryBus.QueryAsync<IGetUserIdentityQuery, IUserIdentity>(new GetUserIdentityQuery {Identifier = identifier});

            IUserIdentity userIdentity = await userIdentityTask;
            if (userIdentity == null)
            {
                return RedirectToAction("UserIdentities", "Security");
            }

            UserIdentityViewModel userIdentityViewModel = _securityViewModelConverter.Convert<IUserIdentity, UserIdentityViewModel>(userIdentity);
            userIdentityViewModel.Claims = BuildClaimViewModelCollection(await systemClaimsTask, userIdentity.ToClaimsIdentity().Claims);
            userIdentityViewModel.EditMode = EditMode.Edit;

            return View("UpdateUserIdentity", userIdentityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserIdentity(UserIdentityViewModel userIdentityViewModel)
        {
            NullGuard.NotNull(userIdentityViewModel, nameof(userIdentityViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateUserIdentity", userIdentityViewModel);
            }

            IUpdateUserIdentityCommand command = _securityViewModelConverter.Convert<UserIdentityViewModel, UpdateUserIdentityCommand>(userIdentityViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("UserIdentities", "Security");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserIdentity(int identifier)
        {
            IDeleteUserIdentityCommand command = new DeleteUserIdentityCommand
            {
                Identifier = identifier
            };
            await _commandBus.PublishAsync<IDeleteUserIdentityCommand>(command);

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
                Claims = BuildClaimViewModelCollection(systemClaims, new List<Claim>(0)),
                EditMode = EditMode.Create
            };

            return View("CreateClientSecretIdentity", clientSecretIdentityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public async Task<IActionResult> UpdateClientSecretIdentity(int identifier)
        {
            Task<IEnumerable<Claim>> systemClaimsTask = _queryBus.QueryAsync<EmptyQuery, IEnumerable<Claim>>(new EmptyQuery());
            Task<IClientSecretIdentity> clientSecretIdentityTask = _queryBus.QueryAsync<IGetClientSecretIdentityQuery, IClientSecretIdentity>(new GetClientSecretIdentityQuery {Identifier = identifier});

            IClientSecretIdentity clientSecretIdentity = await clientSecretIdentityTask;
            if (clientSecretIdentity == null)
            {
                return RedirectToAction("ClientSecretIdentities", "Security");
            }

            ClientSecretIdentityViewModel clientSecretIdentityViewModel = _securityViewModelConverter.Convert<IClientSecretIdentity, ClientSecretIdentityViewModel>(clientSecretIdentity);
            clientSecretIdentityViewModel.Claims = BuildClaimViewModelCollection(await systemClaimsTask, clientSecretIdentity.ToClaimsIdentity().Claims);
            clientSecretIdentityViewModel.EditMode = EditMode.Edit;

            return View("UpdateClientSecretIdentity", clientSecretIdentityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClientSecretIdentity(ClientSecretIdentityViewModel clientSecretIdentityViewModel)
        {
            NullGuard.NotNull(clientSecretIdentityViewModel, nameof(clientSecretIdentityViewModel));

            if (ModelState.IsValid == false)
            {
                return View("UpdateClientSecretIdentity", clientSecretIdentityViewModel);
            }

            IUpdateClientSecretIdentityCommand command = _securityViewModelConverter.Convert<ClientSecretIdentityViewModel, UpdateClientSecretIdentityCommand>(clientSecretIdentityViewModel);
            await _commandBus.PublishAsync(command);

            return RedirectToAction("ClientSecretIdentities", "Security");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClientSecretIdentity(int identifier)
        {
            IDeleteClientSecretIdentityCommand command = new DeleteClientSecretIdentityCommand
            {
                Identifier = identifier
            };
            await _commandBus.PublishAsync<IDeleteClientSecretIdentityCommand>(command);

            return RedirectToAction("ClientSecretIdentities", "Security");
        }

        private List<ClaimViewModel> BuildClaimViewModelCollection(IEnumerable<Claim> systemClaims, IEnumerable<Claim> identityClaims)
        {
            NullGuard.NotNull(systemClaims, nameof(systemClaims))
                .NotNull(identityClaims, nameof(identityClaims));

            List<ClaimViewModel> claimViewModelCollection = _securityViewModelConverter.Convert<IEnumerable<Claim>, IEnumerable<ClaimViewModel>>(systemClaims).OrderBy(m => m.ClaimType).ToList();
            foreach (Claim identityClaim in identityClaims)
            {
                ClaimViewModel claimViewModel = claimViewModelCollection.SingleOrDefault(m => string.CompareOrdinal(m.ClaimType, identityClaim.Type) == 0);
                if (claimViewModel == null)
                {
                    continue;
                }
                claimViewModel.IsSelected = true;
                claimViewModel.ActualValue = string.IsNullOrWhiteSpace(identityClaim.Value) ? claimViewModel.ActualValue : identityClaim.Value;
            }
            return claimViewModelCollection;
        }

        #endregion
    }
}