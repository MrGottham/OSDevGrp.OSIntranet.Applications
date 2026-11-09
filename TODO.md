# Implementing WebApi logic to expose contacts

## General informations

We gona implement logic in the following projects:

* OSDevGrp.OSIntranet.WebApi
* OSDevGrp.OSIntranet.WebApi.Tests
* OSDevGrp.OSIntranet.WebApi.ClientApi which will be automatically be updated by the OSDevGrp.OSIntranet.WebApi.PostBuild

## Implement a contact controller in the WebApi

### Business goal

In the contact controller we gona expose contact groups, country codes, postal codes and payment terms. Furthermore we gona share some entities like payment terms between the contract and accounting controller also the letter heads should be shared between the common controller and the accounting controller.

## Implementation Bullets

### WebApi

- [ ] Add `[HttpGet("contactgroups")]` `ContactGroupsAsync()` with signature `Task<ActionResult<IEnumerable<ContactGroupModel>>>`; use `IQueryBus` to execute `EmptyQuery` and retrieve `IEnumerable<IContactGroup>` from `GetContactGroupCollectionQueryHandler`.
- [ ] Add `[HttpGet("countries")]` `CountriesAsync()` with signature `Task<ActionResult<IEnumerable<CountryCodeModel>>>`; use `IQueryBus` to execute `EmptyQuery` and retrieve `IEnumerable<ICountry>` from `GetCountryCollectionQueryHandler`.
- [ ] Add `[HttpGet("countries/{countryCode}/postalcodes")]` `PostalCodesAsync(string countryCode)` with signature `Task<ActionResult<IEnumerable<PostalCodeModel>>>`; use `IQueryBus` to execute `GetPostalCodeCollectionQuery` with the route country code and retrieve `IEnumerable<IPostalCode>` from `GetPostalCodeCollectionQueryHandler`.
- [ ] Add `[HttpGet("paymentterms")]` `PaymentTermsAsync()` with signature `Task<ActionResult<IEnumerable<PaymentTermModel>>>`; use `IQueryBus` to execute `EmptyQuery` and retrieve `IEnumerable<IPaymentTerm>` from `GetPaymentTermCollectionQueryHandler`.
- [ ] Add `[HttpGet("letterheads")]` `LetterHeadsAsync()` to AccountingController with signature `Task<ActionResult<IEnumerable<LetterHeadModel>>>`; use `IQueryBus` to execute `EmptyQuery` and retrieve `IEnumerable<ILetterHead>` from `GetLetterHeadCollectionQueryHandler`.
- [ ] Add `ContactGroupModel` based on `IContactGroup`, with `Number` required and ranged from 1 to 99 and `Name` required with length 1 to 256, matching the validation attributes on `ContactGroupViewModel`; add its mapping to the contact model converter.
- [ ] Add `CountryCodeModel` based on `ICountry`, matching `CountryViewModel` validation: required `Code` (1-4 characters and country-code pattern), `Name` and `UniversalName` (1-256 characters each), and required `PhonePrefix` (1-4 characters and phone-prefix pattern); do not expose `DefaultForPrincipal`.
- [ ] Add country-code and phone-prefix regex constants to WebApi `ValidationRegexPatterns`, reusing the existing country-code and phone-prefix patterns used by MVC rather than referencing MVC model code.
- [ ] Add `PostalCodeModel` based on `IPostalCode`, with a `Country` reference represented by `CountryCodeModel`; match `PostalCodeViewModel` validation for `Code` (required, 1-16 characters, postal-code pattern), `City` (required, 1-256 characters), and optional `State` (maximum 256 characters); add its mapping to the contact model converter.
- [ ] Add the postal-code regex constant to WebApi `ValidationRegexPatterns`, reusing the existing postal-code pattern used by MVC rather than referencing MVC model code.
- [ ] Map contact groups by number, countries by code, postal codes by code, and payment terms by number for deterministic API responses.
- [ ] Move `PaymentTermModel.cs` from `Models/Accounting` into a new `Models/Shared` folder and update its namespace to `OSDevGrp.OSIntranet.WebApi.Models.Shared`.
- [ ] Move `LetterHeadModel.cs` and `LetterHeadIdentificationModel.cs` from `Models/Common` into `Models/Shared` and update their namespaces to `OSDevGrp.OSIntranet.WebApi.Models.Shared`.
- [ ] Add `SharedModelConverter` mappings for `IPaymentTerm` to `PaymentTermModel`, and `ILetterHead` to both `LetterHeadModel` and `LetterHeadIdentificationModel`; expose the converter through `IConverterFactory` and `ConverterFactory`.
- [ ] Update ContactController and AccountingController payment-term actions, `ContactAccountModel`, `AccountingModelConverter`, `CommonController`, and all affected model references to use the shared models and `SharedModelConverter`; remove duplicate mappings and retire `CommonModelConverter` / `CreateCommonModelConverter` if no longer needed.
- [ ] Preserve the existing accounting payment-terms and common letter-head routes and their response contracts while using shared model types and mapping logic.
- [ ] Add query and mapping dependencies to ContactController using the existing controller and converter-factory patterns, while preserving its contacts authorization policy, API version, and route.

### WebApi.Tests

- [ ] Add focused ContactController action tests with one behavior per test method: separately verify the query type/arguments, `OkObjectResult`, and mapped response content for each action.
- [ ] Test that postal-code lookup passes the requested country code in its own test method; test deterministic collection ordering separately for each applicable action.
- [ ] Add AccountingController `LetterHeadsAsync` tests, with one behavior per test method, verifying the letter-head query, `OkObjectResult`, and mapped response content separately.
- [ ] Add or update converter tests for contact-group, country, postal-code, payment-term, and letter-head mappings, including shared model reuse.
- [ ] Keep existing AccountingController payment-term and CommonController letter-head behavior covered and unchanged.

### WebApi.ClientApi

- [ ] Build the WebApi project to regenerate the client API through its existing post-build process, then verify the generated client exposes the new contact operations and shared response models; do not hand-edit generated files.
