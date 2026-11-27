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

## Step 2: Expose the three read-only contact endpoints (iteration A)

**Author:** main

### Prompt Context

**Verbatim prompt:** "Lets implement this"
**Interpretation:** Implement iteration A of the PRD: `ContactsAsync`, `ContactAsync(externalIdentifier)`, and `ContactsWithBirthdayAsync(withinDays)` on `ContactController`, plus tests.
**Inferred intent:** Make the contact read model reachable through the WebApi with the same authorization model as the MVC app.

### What I did
- Added the three actions to `/OSDevGrp.OSIntranet.WebApi/Controllers/ContactController.cs`. Each has `[Authorize(Policy = Policies.ContactsFromExternalProviderPolicy)]`.
- `externalIdentifier` carries `[Required]` and `[StringLength(256, MinimumLength = 1)]`, matching `ContactModel.ExternalIdentifier`. `withinDays` carries `[Required]` and `[Range(0, 365)]`.
- The Microsoft token is read directly from the `MicrosoftTokenClaimType` claim and parsed there. The shared claim resolver is not reused, because it branches on `ClaimValueType`, which is lost in the ID token round trip.
- An expired Microsoft token returns 401 before any query is built or sent.
- Contacts are ordered by display name (OrdinalIgnoreCase), then by `ExternalIdentifier` (ordinal).
- Regenerated `WebApiClient.generated.cs` with the postbuild. The committed file was already stale, so the diff includes unrelated churn.
- Added focused tests under `/OSDevGrp.OSIntranet.WebApi.Tests/Controllers/ContactController/`: `ContactControllerTestBase.cs`, `ContactsAsyncTests.cs`, `ContactAsyncTests.cs`, `ContactsWithBirthdayAsyncTests.cs`, and additions to `ContactControllerMetadataTests.cs`. Each test checks one behavior.

### What worked
- The WebApi test project builds with 0 errors and 0 warnings.
- `dotnet test` filtered to the `ContactController` namespace with `Category=UnitTest` passes 55 of 55.

### What didn't work
- `dotnet build` on the WebApi project fails in the postbuild step (exit 134) because the build passes `--solutionDir *Undefined*`. Running `dotnet WebApi.dll postbuild --solutionDir .` directly succeeds, and that is how the client was regenerated.
- `RangeAttribute` is ambiguous between DataAnnotations and NUnit in the metadata tests. A using alias resolved it.

### Open
- `ContactAsync` is routed at `{externalIdentifier}` (not `contacts/{externalIdentifier}`) to avoid repeating `contacts` in the path.
- `ContactAsync(api_version)` returns a collection and `ContactsAsync(externalIdentifier)` returns a single contact in the generated client. The names are inverted because the root route is bound to `ContactsAsync`.
- The WebApi has no token refresh flow. MVC refreshes through `AcquireTokenActionFilter`.
- The ID token is signed but not encrypted, so the refresh token is readable from it. This is tracked separately from this iteration.

