# Diary: Expose contacts through the WebApi

Goal: implement iteration 1 from `tasks/2026-11-20-expose-contacts-through-webapi/PRD.md`: WebApi contact models, validation, and the `IContact`/`IName`/`IAddress`/`ICompany` mappings. Controllers and the generated client are out of scope for this iteration.

## Step 1: Add contact models, validation, and converter mappings

**Author:** main

### Prompt Context

**Verbatim prompt:** "Lets implement the plan"
**Interpretation:** Implement the approved iteration 1 plan: contact DTOs with validation, converter mappings, and focused unit tests.
**Inferred intent:** Establish the WebApi contact contract as a reviewable, independently tested slice before controllers expose it.

### What I did
- Added `ContactModel`, `NameModel`, `AddressModel`, and `CompanyModel` under `/OSDevGrp.OSIntranet.WebApi/Models/Contacts/`, with length, regex, and required validation mirroring the MVC contact view model.
- Added `UrlRegexPattern` to `/OSDevGrp.OSIntranet.WebApi/Models/ValidationRegexPatterns.cs`. The WebApi copy fixes the MVC pattern's `(:(0-9)*)*` typo to `(:[0-9]*)*`. The MVC copy is unchanged.
- Extended `/OSDevGrp.OSIntranet.WebApi/Models/Contacts/ContactModelConverter.cs` with `IName`, `IAddress`, `ICompany`, and `IContact` maps. `IName` uses runtime `is IPersonName` / `is ICompanyName` checks, following the MVC converter. `PaymentTerm` goes through the shared converter.
- Split the multi-assertion `PaymentTermsAsync` test in `/OSDevGrp.OSIntranet.WebApi.Tests/Controllers/ContactController/PaymentTermsAsyncTests.cs` so each test method checks one behavior.
- Added one-assertion converter tests in `ConvertTests.cs` and validation tests under `/OSDevGrp.OSIntranet.WebApi.Tests/Models/Contacts/` for `ContactModel`, `NameModel`, `AddressModel`, and `CompanyModel`.

### Why
The models and mappings are the contract that later controller iterations will expose. Keeping them in the WebApi project, with no controller or generated-client changes, keeps main green and reviewable. The one-behavior-per-test rule keeps failures specific.

### What worked
- `dotnet build` of the WebApi project succeeded with no warnings.
- `dotnet test` with `Category=UnitTest` passed all 1,347 tests.
- The generated client `WebApiClient.generated.cs` has no diff, so no API surface was exposed in this iteration.

### What didn't work
- Expression-tree mappings reject `is` pattern declarations (`is IPersonName p`) with CS8122. Type-only `is` plus a cast fixed it.
- The first test build failed because `ConvertTests.cs` lacked `using System;` and `using Moq;`. Adding them fixed it.
