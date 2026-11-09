# PRD: Expose contact data through the WebApi

## Relationship to the existing PRD

This is a follow-up to `tasks/2026-11-06-expose-contacts-webapi/PRD.md`. That PRD establishes the secured, versioned ContactController foundation and policies. This follow-up adds contact-data operations and shared API models without replacing or weakening that foundation.

## Problem

The WebApi has no operations for retrieving contact groups, countries, postal codes, or payment terms. Payment terms and letter heads are also represented as controller-specific models and mappings, making it harder to expose the same response contracts from multiple controllers.

## Goal

Expose the required contact data through the versioned WebApi, add letter-head retrieval to AccountingController, and use shared response models and mappings for payment terms and letter heads. Preserve existing routes, response contracts, and controller security metadata.

## User stories

- As a WebApi consumer, I want to retrieve contact groups, countries, postal codes, and payment terms so that I can populate contact-related workflows.
- As an accounting API consumer, I want to retrieve letter heads from AccountingController while existing common-controller clients continue to work unchanged.
- As a WebApi client generator, I want shared response models for payment terms and letter heads so the same API contracts can be reused consistently.

## Functional requirements

### Contact operations

Add these GET operations to ContactController, using `IQueryBus` and the existing query-handler and converter-factory patterns:

| Route | Action | Query and result |
|---|---|---|
| `contactgroups` | `ContactGroupsAsync()` | Execute `EmptyQuery`; map `IEnumerable<IContactGroup>` |
| `countries` | `CountriesAsync()` | Execute `EmptyQuery`; map `IEnumerable<ICountry>` |
| `countries/{countryCode}/postalcodes` | `PostalCodesAsync(string countryCode)` | Execute `GetPostalCodeCollectionQuery` with the route country code; map `IEnumerable<IPostalCode>` |
| `paymentterms` | `PaymentTermsAsync()` | Execute `EmptyQuery`; map `IEnumerable<IPaymentTerm>` |

Preserve ContactController's existing route, API version, and contacts authorization policy.

### Accounting letter heads and existing operations

- Add `LetterHeadsAsync()` to AccountingController at `letterheads`. Execute `EmptyQuery` and map `IEnumerable<ILetterHead>`.
- Keep the existing accounting payment-terms route and response contract, using the shared payment-term model and mapping.
- Keep the existing common letter-head route and response contract, using the shared letter-head model and mapping.

### Models, validation, and mappings

- Add `ContactGroupModel` based on `IContactGroup`. `Number` is required and ranges from 1 to 99; `Name` is required and has length 1–256, matching `ContactGroupViewModel`.
- Add `CountryCodeModel` based on `ICountry`. `Code` is required, length 1–4, and matches the existing MVC country-code pattern. `Name` and `UniversalName` are required and have length 1–256. `PhonePrefix` is required, length 1–4, and matches the existing MVC phone-prefix pattern. Do not expose `DefaultForPrincipal`.
- Add `PostalCodeModel` based on `IPostalCode`, with `Country` represented by `CountryCodeModel`. `Code` is required, length 1–16, and matches the existing MVC postal-code pattern; `City` is required and has length 1–256; `State` is optional and has a maximum length of 256.
- Define the country-code, phone-prefix, and postal-code regex constants in WebApi `ValidationRegexPatterns`, reusing the existing patterns without referencing MVC model code.
- Add contact-group, country, and postal-code mappings to the contact model converter.
- Order contact groups by number, countries by code, postal codes by code, and payment terms by number before returning collection responses.
- Move `PaymentTermModel`, `LetterHeadModel`, and `LetterHeadIdentificationModel` into `Models/Shared` and update their namespaces to `OSDevGrp.OSIntranet.WebApi.Models.Shared`.
- Add `SharedModelConverter` mappings from `IPaymentTerm` to `PaymentTermModel` and from `ILetterHead` to both letter-head models. Expose the converter through `IConverterFactory` and `ConverterFactory`.
- Update all affected controllers, models, and converters to use the shared models and `SharedModelConverter`. Remove duplicate mappings and retire `CommonModelConverter` / `CreateCommonModelConverter` if they are no longer needed.

## Acceptance criteria

1. ContactController exposes all four operations with the specified routes, signatures, query types, query arguments, and model response types.
2. Postal-code lookup passes the requested country code to `GetPostalCodeCollectionQuery`.
3. Contact collection responses are ordered by the specified keys: contact-group number, country code, postal-code code, and payment-term number.
4. The new contact models enforce the stated validation constraints and map from their corresponding domain interfaces. Country responses do not expose `DefaultForPrincipal`.
5. WebApi owns the validation regex constants and uses the same country-code, phone-prefix, and postal-code patterns as MVC without depending on MVC model code.
6. AccountingController exposes `LetterHeadsAsync()` using the letter-head collection query and shared letter-head model mapping.
7. Existing accounting payment-term and common letter-head routes and response contracts remain unchanged while using the shared models and mapping logic.
8. Shared payment-term and letter-head models reside in `Models/Shared`, and the shared mappings are available through the converter factory without duplicate obsolete mappings.
9. ContactController retains its contacts authorization policy, API version, and route.
10. Focused WebApi.Tests verify each ContactController action's query type/arguments, `OkObjectResult`, mapped response content, and applicable ordering in separate behavior-specific tests. Postal-code country-code argument verification is its own test.
11. Focused AccountingController tests verify the letter-head query, `OkObjectResult`, and mapped response content in separate behavior-specific tests. Converter tests cover contact-group, country, postal-code, payment-term, and letter-head mappings, including shared-model reuse. Existing accounting payment-term and common letter-head behavior remains covered.
12. Building WebApi runs its existing post-build client generation process. The generated client exposes the new contact operations and shared response models; generated files are not hand-edited.

## Scope

### In scope

- Contact-group, country, postal-code, and payment-term GET operations.
- Accounting letter-head retrieval.
- Contact and shared API models, validation, mappings, and collection ordering.
- Focused WebApi.Tests coverage and generated-client verification.

### Out of scope

- Changes to the existing contacts authorization policy, API version, or ContactController route.
- Changes to the existing accounting payment-term or common letter-head routes or response contracts.
- Hand-editing generated WebApi.ClientApi files.
- New operations beyond those listed above.

## Risks and constraints

- Moving model types changes their namespaces. All controller, converter, and test references must be updated together to keep the generated contracts consistent.
- WebApi validation patterns must remain equivalent to MVC patterns without introducing a dependency on MVC model code.
- WebApi's post-build process regenerates the client API; build WebApi before validating WebApi.ClientApi, and do not edit generated output manually.
