# PRD: Minimize country data in postal-code responses

## Problem

Postal-code responses currently represent their `Country` with `CountryCodeModel`, which includes `UniversalName` and `PhonePrefix` in addition to the identifying `Code` and `Name`. Consumers of postal-code data receive country fields that are not needed for identifying the country. The country-list response still needs to provide the full country data.

## Relevant Codebase

- **What's there**:
  - `OSDevGrp.OSIntranet.WebApi/Models/Contacts/CountryCodeModel.cs:6` defines `CountryCodeModel` with `Code`, `Name`, `UniversalName`, and `PhonePrefix`; the properties carry required, length, regex, and JSON-required validation metadata.
  - `OSDevGrp.OSIntranet.WebApi/Models/Contacts/PostalCodeModel.cs:6` defines `PostalCodeModel`; its `Country` property at line 10 currently has type `CountryCodeModel`.
  - `OSDevGrp.OSIntranet.WebApi/Controllers/ContactController.cs:68` returns country-list results as `CountryCodeModel`. `PostalCodesAsync` at line 81 returns `PostalCodeModel` results.
  - `OSDevGrp.OSIntranet.WebApi/Models/Contacts/ContactModelConverter.cs:23` maps `ICountry` to `CountryCodeModel`, and line 24 maps `IPostalCode` to `PostalCodeModel`.
  - Converter coverage in `OSDevGrp.OSIntranet.WebApi.Tests/Models/Contacts/ContactModelConverter/ConvertTests.cs:100` checks country mapping within a postal-code result. Country model validation is covered in `OSDevGrp.OSIntranet.WebApi.Tests/Models/Contacts/CountryCodeModel/ValidationTests.cs`; postal-code validation is covered in `OSDevGrp.OSIntranet.WebApi.Tests/Models/Contacts/PostalCodeModel/ValidationTests.cs`.
  - The API client in `OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs` is generated from the WebApi build; its current `PostalCodeModel.Country` property uses `CountryCodeModel`.
- **How it works**: `ContactController.CountriesAsync` queries countries and converts them to `CountryCodeModel`. `PostalCodesAsync` queries postal codes and converts each `IPostalCode` to `PostalCodeModel`; AutoMapper maps the postal code's country into the `Country` member. The member's declared model type determines the country fields represented by that response contract.
- **Patterns to follow**: Keep the existing data annotations and JSON-required metadata associated with `Code` and `Name` when they move to the identification model. Continue using the contact model converter for domain-to-API-model mappings and the existing focused WebApi.Tests conventions.
- **Integration points**: The API models affect both response endpoints, converter mappings, validation/converter tests, and the generated WebApi client contract. Building WebApi runs the existing client generation process; generated files are not hand-edited.

## Goal

Represent the shared identifying country fields in a new `CountryCodeIdentificationModel` containing `Code` and `Name`. Make `CountryCodeModel` inherit from it and retain `UniversalName` and `PhonePrefix`. Use `CountryCodeIdentificationModel` for `PostalCodeModel.Country`, so postal-code responses expose only the identifying country fields while country-list responses continue exposing the full country model.

## User Stories

- As a consumer of postal-code data, I want the nested country to contain only its code and name so that the response exposes only the country information needed for identification.
- As a consumer of the countries endpoint, I want country responses to retain universal name and phone prefix as well as code and name so that existing country-related use cases keep their full data.

## Acceptance Criteria

1. `CountryCodeIdentificationModel` is defined in the WebApi contacts models and contains `Code` and `Name`, retaining their existing required, length, regex (for `Code`), and JSON-required metadata.
2. `CountryCodeModel` inherits from `CountryCodeIdentificationModel` and retains `UniversalName` and `PhonePrefix` with their existing validation and JSON metadata.
3. `PostalCodeModel.Country` is typed as `CountryCodeIdentificationModel`.
4. Converting a country for the countries endpoint continues to populate `Code`, `Name`, `UniversalName`, and `PhonePrefix`.
5. Converting a postal code populates the nested country's `Code` and `Name`; the serialized postal-code response's nested country does not include `UniversalName` or `PhonePrefix`.
6. The existing country and postal-code validation and mapping tests are updated or extended to verify the new model hierarchy, metadata, and response fields.
7. Building WebApi regenerates the client contract so that `CountriesAsync` still exposes the full `CountryCodeModel` and `PostalCodeModel.Country` uses the reduced identification model. The generated client file is not hand-edited.

## Scope

### In scope

- Add the country identification model and move the existing `Code` and `Name` properties and metadata into it.
- Make `CountryCodeModel` inherit the identification model while retaining its country-specific fields.
- Change the postal-code country member type, conversion behavior, focused tests, and generated client contract accordingly.

### Out of scope

- Changing endpoint routes, query behavior, sorting, or authorization.
- Removing or changing fields from the countries endpoint response.
- Changes to domain or repository country and postal-code models.

## Risks

- The nested country is mapped as part of the `IPostalCode` to `PostalCodeModel` conversion. The destination model type changes, so the mapping must populate the new identification model without returning a full `CountryCodeModel` instance in the nested response.
- The generated client must reflect both model shapes: full country data in the countries response and only identifying fields in a postal-code country's response.
