﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Private variables

        private readonly IQueryBus _queryBus;
        private readonly IClaimResolver _claimResolver;
        private readonly ITokenHelperFactory _tokenHelperFactory;
        private readonly IAcmeChallengeResolver _acmeChallengeResolver;
        private readonly IConverter _homeViewModelConverter = new HomeViewModelConverter();

        #endregion

        #region Constructor

        public HomeController(IQueryBus queryBus, IClaimResolver claimResolver, ITokenHelperFactory tokenHelperFactory, IAcmeChallengeResolver acmeChallengeResolver)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus))
                .NotNull(claimResolver, nameof(claimResolver))
                .NotNull(tokenHelperFactory, nameof(tokenHelperFactory))
                .NotNull(acmeChallengeResolver, nameof(acmeChallengeResolver));

            _queryBus = queryBus;
            _claimResolver = claimResolver;
            _tokenHelperFactory = tokenHelperFactory;
            _acmeChallengeResolver = acmeChallengeResolver;
        }

        #endregion

        #region Methods

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return View("Index", BuildHomeOperationsViewModelForUnauthenticatedUser());
            }

            return View("Index", await BuildHomeOperationsViewModelForAuthenticatedUserAsync(User));
        }

        [HttpGet]
        [Authorize(Policy = Policies.ContactPolicy)]
        public async Task<IActionResult> UpcomingBirthdays(int withinDays)
        {
            IRefreshableToken token = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
            if (token == null)
            {
                return Unauthorized();
            }

            IGetContactWithBirthdayCollectionQuery query = new GetContactWithBirthdayCollectionQuery
            {
                BirthdayWithinDays = withinDays,
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Expires = token.Expires
            };
            IEnumerable<IContact> contacts = await _queryBus.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(query);

            List<ContactWithUpcomingBirthdayViewModel> contactWithUpcomingBirthdayViewModels = contacts.AsParallel()
                .Where(contact => contact.Birthday.HasValue)
                .Select(_homeViewModelConverter.Convert<IContact, ContactWithUpcomingBirthdayViewModel>)
                .OrderBy(contactWithUpcomingBirthdayViewModel => contactWithUpcomingBirthdayViewModel.UpcomingBirthday)
                .ThenBy(contactWithUpcomingBirthdayViewModel => contactWithUpcomingBirthdayViewModel.DisplayName)
                .ToList();

            return PartialView("_UpcomingBirthdayCollectionPartial", contactWithUpcomingBirthdayViewModels);
        }

        [HttpGet]
        [Authorize(Policy = Policies.AccountingPolicy)]
        public async Task<IActionResult> AccountingInformation(int accountingNumber)
        {
            IGetAccountingQuery query = new GetAccountingQuery
            {
                AccountingNumber = accountingNumber,
                StatusDate = DateTime.Today
            };
            IAccounting accounting = await _queryBus.QueryAsync<IGetAccountingQuery, IAccounting>(query);
            if (accounting == null)
            {
                return BadRequest();
            }

            AccountingPresentationViewModel accountingPresentationViewModel = _homeViewModelConverter.Convert<IAccounting, AccountingPresentationViewModel>(accounting);

            return PartialView("_AccountingPresentationPartial", accountingPresentationViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CollectNews(int numberOfNewsToCollect)
        {
            IGetNewsCollectionQuery query = new GetNewsCollectionQuery(true, numberOfNewsToCollect);
            IEnumerable<INews> news = await _queryBus.QueryAsync<IGetNewsCollectionQuery, IEnumerable<INews>>(query);

            IEnumerable<ExternalNewsViewModel> externalNewsViewModels = _homeViewModelConverter.Convert<IEnumerable<INews>, IEnumerable<ExternalNewsViewModel>>(news ?? Array.Empty<INews>())
                .OrderByDescending(externalNewsViewModel => externalNewsViewModel.Timestamp)
                .ToArray();

            return PartialView("_ExternalNewsCollectionPartial", externalNewsViewModels);
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error == null)
            {
                return View(new ErrorViewModel {RequestId = requestId});
            }

            return ExceptionToView(exceptionHandlerPathFeature.Error, requestId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/.well-known/acme-challenge/{challengeToken}")]
        public IActionResult AcmeChallenge(string challengeToken)
        {
            NullGuard.NotNullOrWhiteSpace(challengeToken, nameof(challengeToken));

            string constructedKeyAuthorization = _acmeChallengeResolver.GetConstructedKeyAuthorization(challengeToken);
            if (string.IsNullOrWhiteSpace(constructedKeyAuthorization))
            {
                return BadRequest();
            }

            return File(Encoding.UTF8.GetBytes(constructedKeyAuthorization), "application/octet-stream");
        }

        private HomeOperationsViewModel BuildHomeOperationsViewModelForUnauthenticatedUser()
        {
            return new HomeOperationsViewModel
            {
                CanAccessContacts = false,
                HasAcquiredMicrosoftGraphToken = false,
                UpcomingBirthdaysWithinDays = default,
                CanAccessAccountings = false,
                AccountingNumber = default,
                NumberOfNewsToCollect = 10
            };
        }

        private async Task<HomeOperationsViewModel> BuildHomeOperationsViewModelForAuthenticatedUserAsync(ClaimsPrincipal user)
        {
            NullGuard.NotNull(user, nameof(user));

            bool canAccessContacts = User.HasClaim(claim => claim.Type == ClaimHelper.ContactsClaimType);
            bool hasAcquiredMicrosoftGraphToken = false;
            int upcomingBirthdaysWithinDays = default;
            if (canAccessContacts)
            {
                IRefreshableToken microsoftGraphToken = await _tokenHelperFactory.GetTokenAsync<IRefreshableToken>(TokenType.MicrosoftGraphToken, HttpContext);
                hasAcquiredMicrosoftGraphToken = microsoftGraphToken != null && microsoftGraphToken.WillExpireWithin(new TimeSpan(0, 0, 1, 0, 0)) == false;
                upcomingBirthdaysWithinDays = hasAcquiredMicrosoftGraphToken ? 14 : default;
            }

            bool canAccessAccountings = User.HasClaim(claim => claim.Type == ClaimHelper.AccountingClaimType);
            int? accountingNumber = default;
            if (canAccessAccountings)
            {
                accountingNumber = _claimResolver.GetAccountingNumber();
            }

            bool collectNews = User.HasClaim(claim => claim.Type == ClaimHelper.CollectNewsClaimType);
            int? numberOfNewsToCollect = default;
            if (collectNews)
            {
                numberOfNewsToCollect = _claimResolver.GetNumberOfNewsToCollect();
            }

            return new HomeOperationsViewModel
            {
                CanAccessContacts = canAccessContacts,
                HasAcquiredMicrosoftGraphToken = hasAcquiredMicrosoftGraphToken,
                UpcomingBirthdaysWithinDays = upcomingBirthdaysWithinDays,
                CanAccessAccountings = canAccessAccountings,
                AccountingNumber = accountingNumber,
                NumberOfNewsToCollect = numberOfNewsToCollect
            };
        }

        private IActionResult ExceptionToView(Exception exception, string requestId)
        {
            NullGuard.NotNull(exception, nameof(exception));

            IntranetExceptionBase intranetException = exception as IntranetBusinessException;
            if (intranetException != null)
            {
                return ExceptionToView(intranetException, requestId);
            }

            if (exception is AggregateException aggregateException)
            {
                return ExceptionToView(aggregateException, requestId);
            }

            return View(new ErrorViewModel {RequestId = requestId});
        }

        private IActionResult ExceptionToView(IntranetExceptionBase intranetException, string requestId)
        {
            NullGuard.NotNull(intranetException, nameof(intranetException));

            if (intranetException is IntranetBusinessException intranetBusinessException)
            {
                return View(new ErrorViewModel {RequestId = requestId, ErrorCode = (int) intranetBusinessException.ErrorCode, ErrorMesssage = intranetBusinessException.Message});
            }

            return View(new ErrorViewModel {RequestId = requestId});
        }

        private IActionResult ExceptionToView(AggregateException aggregateException, string requestId)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException));

            Exception exception = null;
            aggregateException.Handle(ex => 
            {
                exception = ex;
                return true;
            });

            return ExceptionToView(exception, requestId);
        }

        #endregion
    }
}