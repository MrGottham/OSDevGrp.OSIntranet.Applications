using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.WebApi.Helpers.Factories;
using OSDevGrp.OSIntranet.WebApi.Models;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Models.Shared;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [Authorize(Policy = Policies.ContactPolicy)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        #region Private variables

        private readonly IQueryBus _queryBus;
        private readonly IConverter _contactModelConverter;
        private readonly IConverter _sharedModelConverter;

        #endregion

        #region Constructor

        public ContactController(IQueryBus queryBus, IConverterFactory converterFactory)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus))
                .NotNull(converterFactory, nameof(converterFactory));

            _queryBus = queryBus;
            _contactModelConverter = converterFactory.CreateContactModelConverter();
            _sharedModelConverter = converterFactory.CreateSharedModelConverter();
        }

        #endregion

        #region Methods

        [HttpGet("contactgroups")]
        public async Task<ActionResult<IEnumerable<ContactGroupModel>>> ContactGroupsAsync()
        {
            IEnumerable<IContactGroup> contactGroups = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(new EmptyQuery());

            IEnumerable<ContactGroupModel> contactGroupModels = contactGroups.AsParallel()
                .Select(_contactModelConverter.Convert<IContactGroup, ContactGroupModel>)
                .OrderBy(contactGroupModel => contactGroupModel.Number)
                .ToList();

            return new OkObjectResult(contactGroupModels);
        }

        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<CountryCodeModel>>> CountriesAsync()
        {
            IEnumerable<ICountry> countries = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(new EmptyQuery());

            IEnumerable<CountryCodeModel> countryCodeModels = countries.AsParallel()
                .Select(_contactModelConverter.Convert<ICountry, CountryCodeModel>)
                .OrderBy(countryCodeModel => countryCodeModel.Code)
                .ToList();

            return new OkObjectResult(countryCodeModels);
        }

        [HttpGet("countries/{countryCode}/postalcodes")]
        public async Task<ActionResult<IEnumerable<PostalCodeModel>>> PostalCodesAsync(
            [Required]
            [StringLength(4, MinimumLength = 1)]
            [RegularExpression(ValidationRegexPatterns.CountryCodeRegexPattern)]
            string countryCode)
        {
            IGetPostalCodeCollectionQuery query = new GetPostalCodeCollectionQuery
            {
                CountryCode = countryCode
            };

            IEnumerable<IPostalCode> postalCodes = await _queryBus.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(query);

            IEnumerable<PostalCodeModel> postalCodeModels = postalCodes.AsParallel()
                .Select(_contactModelConverter.Convert<IPostalCode, PostalCodeModel>)
                .OrderBy(postalCodeModel => postalCodeModel.Code)
                .ToList();

            return new OkObjectResult(postalCodeModels);
        }

        [HttpGet("paymentterms")]
        public async Task<ActionResult<IEnumerable<PaymentTermModel>>> PaymentTermsAsync()
        {
            IEnumerable<IPaymentTerm> paymentTerms = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(new EmptyQuery());

            IEnumerable<PaymentTermModel> paymentTermModels = paymentTerms.AsParallel()
                .Select(_sharedModelConverter.Convert<IPaymentTerm, PaymentTermModel>)
                .OrderBy(paymentTermModel => paymentTermModel.Number)
                .ToList();

            return new OkObjectResult(paymentTermModels);
        }

        [HttpGet]
        [HttpGet("contacts")]
        [Authorize(Policy = Policies.ContactsFromExternalProviderPolicy)]
        public async Task<ActionResult<IEnumerable<ContactModel>>> ContactsAsync()
        {
            IRefreshableToken refreshableToken = GetMicrosoftToken();
            if (refreshableToken == null)
            {
                return Unauthorized();
            }

            IGetContactCollectionQuery query = CreateContactQuery<GetContactCollectionQuery>(refreshableToken);

            IEnumerable<IContact> contacts = await _queryBus.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(query);

            return new OkObjectResult(ConvertContacts(contacts));
        }

        [HttpGet("{externalIdentifier}")]
        [Authorize(Policy = Policies.ContactsFromExternalProviderPolicy)]
        public async Task<ActionResult<ContactModel>> ContactAsync(
            [Required]
            [StringLength(256, MinimumLength = 1)]
            string externalIdentifier)
        {
            IRefreshableToken refreshableToken = GetMicrosoftToken();
            if (refreshableToken == null)
            {
                return Unauthorized();
            }

            IGetContactQuery query = CreateContactQuery<GetContactQuery>(refreshableToken);
            query.ExternalIdentifier = externalIdentifier;

            IContact contact = await _queryBus.QueryAsync<IGetContactQuery, IContact>(query);
            if (contact == null)
            {
                return NotFound();
            }

            return new OkObjectResult(_contactModelConverter.Convert<IContact, ContactModel>(contact));
        }

        [HttpGet("withbirthdays")]
        [Authorize(Policy = Policies.ContactsFromExternalProviderPolicy)]
        public async Task<ActionResult<IEnumerable<ContactModel>>> ContactsWithBirthdayAsync(
            [Required]
            [Range(0, 365)]
            int withinDays)
        {
            IRefreshableToken refreshableToken = GetMicrosoftToken();
            if (refreshableToken == null)
            {
                return Unauthorized();
            }

            IGetContactWithBirthdayCollectionQuery query = CreateContactQuery<GetContactWithBirthdayCollectionQuery>(refreshableToken);
            query.BirthdayWithinDays = withinDays;

            IEnumerable<IContact> contacts = await _queryBus.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(query);

            return new OkObjectResult(ConvertContacts(contacts));
        }

        #endregion

        #region Private methods

        private IRefreshableToken GetMicrosoftToken()
        {
            string tokenValue = User.GetClaim(ClaimHelper.MicrosoftTokenClaimType)?.Value;
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                return null;
            }

            try
            {
                IRefreshableToken refreshableToken = RefreshableTokenFactory.Create().FromBase64String(tokenValue);

                return refreshableToken == null || refreshableToken.HasExpired ? null : refreshableToken;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private IEnumerable<ContactModel> ConvertContacts(IEnumerable<IContact> contacts)
        {
            return contacts.OrderBy(contact => contact.Name?.DisplayName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ThenBy(contact => contact.ExternalIdentifier, StringComparer.Ordinal)
                .Select(_contactModelConverter.Convert<IContact, ContactModel>)
                .ToList();
        }

        private static T CreateContactQuery<T>(IRefreshableToken refreshableToken) where T : class, IContactQuery, new()
        {
            return new T
            {
                TokenType = refreshableToken.TokenType,
                AccessToken = refreshableToken.AccessToken,
                RefreshToken = refreshableToken.RefreshToken,
                Expires = refreshableToken.Expires
            };
        }

        #endregion
    }
}