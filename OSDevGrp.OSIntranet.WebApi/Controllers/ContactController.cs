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
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.WebApi.Helpers.Factories;
using OSDevGrp.OSIntranet.WebApi.Models;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Security;
using System.Collections.Generic;
using System.Linq;
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

        #endregion

        #region Constructor

        public ContactController(IQueryBus queryBus, IConverterFactory converterFactory)
        {
            NullGuard.NotNull(queryBus, nameof(queryBus))
                .NotNull(converterFactory, nameof(converterFactory));

            _queryBus = queryBus;
            _contactModelConverter = converterFactory.CreateContactModelConverter();
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

        #endregion
    }
}