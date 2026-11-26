# PRD: Expose contacts through the WebApi

## Problem

The WebApi's existing `ContactController` exposes lookup data but not contacts themselves. API consumers need authorized access to contacts obtained from the Microsoft external provider, including a contact lookup and a birthday-window collection.

## Relevant Codebase

- **What's there**:
  - `OSDevGrp.OSIntranet.WebApi/Controllers/ContactController.cs:24-28` defines the version 1.0 controller at `api/[controller]`, with controller-level `ContactPolicy`; its existing lookup actions are at lines 55-111.
  - `OSDevGrp.OSIntranet.WebApi/Security/Policies.cs:13-14` defines both `ContactPolicy` and `ContactsFromExternalProviderPolicy`. `OSDevGrp.OSIntranet.WebApi/Startup.cs:214-225` registers the policies; the external-provider policy requires an authenticated user and both contacts and Microsoft-token claims.
  - `OSDevGrp.OSIntranet.Domain.Interfaces/Contacts/IContact.cs:8-48` defines contact data and domain methods. No `ContactModel` currently exists in the WebApi. `ContactModelConverter` maps contact groups and other contact lookup models (`OSDevGrp.OSIntranet.WebApi/Models/Contacts/ContactModelConverter.cs:22-25`); `SharedModelConverter` maps `IPaymentTerm` to the existing WebApi `PaymentTermModel` (`OSDevGrp.OSIntranet.WebApi/Models/Shared/SharedModelConverter.cs:29`).
  - MVC's `ContactViewModel`, `ContactInfoViewModel`, `ContactIdentificationViewModel`, and nested models contain validation attributes that serve as the reference for WebApi models. `ExternalIdentifier` has no MVC data annotation (`OSDevGrp.OSIntranet.Mvc/Models/Contacts/ContactIdentificationViewModel.cs:11`); business-logic validation separately rejects null or whitespace identifiers (`OSDevGrp.OSIntranet.BusinessLogic/Contacts/Logic/ExternalIdentifierHelper.cs:14-23`).
  - `IClaimResolver.GetMicrosoftToken(IPrincipal, Func<string, string>)` resolves and unprotects the Microsoft token claim (`OSDevGrp.OSIntranet.BusinessLogic/Security/Logic/ClaimResolver.cs:102-107`). Tokens expose `HasExpired` (`OSDevGrp.OSIntranet.Domain.Interfaces/Security/IToken.cs:5-13`), and the WebApi's security controller uses the `AuthorizationStateProtection` data protector (`OSDevGrp.OSIntranet.WebApi/Controllers/SecurityController.cs:158-160`).
  - The contact queries already exist. Their handlers use the Microsoft Graph repository and apply contact supplements; the birthday handler filters with `IContact.HasBirthdayWithinDays` (`OSDevGrp.OSIntranet.BusinessLogic/Contacts/QueryHandlers/GetContactCollectionQueryHandler.cs:27-39`, `GetContactQueryHandler.cs:26-36`, and `GetContactWithBirthdayCollectionQueryHandler.cs:27-48`). The single-contact handler returns null when Graph finds no match.
  - WebApi controller tests use NUnit, with current action and authorization metadata coverage in `OSDevGrp.OSIntranet.WebApi.Tests/Controllers/ContactController/ContactControllerMetadataTests.cs` and action test patterns in `ContactGroupsAsyncTests.cs`.
  - `OSDevGrp.OSIntranet.WebApi/OSDevGrp.OSIntranet.WebApi.csproj:39-46` runs the existing post-build client generation.
- **How it works**: Each new action resolves the current user's Microsoft refreshable token from the protected claim, rejects a missing or expired token before querying, and passes its token fields to the existing query bus. Query handlers obtain contacts from Microsoft Graph, supplement them from the contact repository, and the birthday handler applies the domain birthday-window predicate. Returned contacts are converted to WebApi models and ordered before the response is returned.
- **Patterns to follow**: Keep WebApi DTOs independent of MVC view models. Use the existing contact/shared converters, query bus, authorization policies, token resolver, controller route/version conventions, and focused NUnit tests. Preserve controller-level authorization for existing actions.
- **Integration points**: The controller depends on the existing contact query contracts, `IQueryBus`, `IClaimResolver`, data protection, and model converters. Building the WebApi regenerates the client from its API description.

## Goal

Provide three authorized WebApi operations for listing contacts, retrieving one contact by external identifier, and listing contacts with birthdays within a requested number of days. Contact data is returned in a dedicated WebApi model, and the generated client exposes these operations.

## User Stories

- As an authorized WebApi consumer, I want to retrieve all contacts so that I can use the contacts available from the external provider.
- As an authorized WebApi consumer, I want to retrieve a contact by its external identifier so that I can look up a specific contact.
- As an authorized WebApi consumer, I want to retrieve contacts with birthdays within a specified number of days so that I can identify upcoming birthdays.

## Acceptance Criteria

1. `ContactModel` exposes every `IContact` data property except `InternalIdentifier`, does not expose domain methods, and uses dedicated WebApi models for `Name`, `Address`, `Company`, `ContactGroup`, and `PaymentTerm`. Existing compatible WebApi models are reused; MVC view models are not reused and the WebApi does not gain a dependency on MVC.
2. `ContactModel` and its nested WebApi models have relevant validation attributes based on the MVC model equivalents. `ContactModel.ExternalIdentifier` and the `ContactAsync` route parameter both have `[Required]`.
3. `ContactModelConverter` maps `IContact` and each required interface-valued property to its WebApi model.
4. Each new action declares `ContactsFromExternalProviderPolicy`, while existing `ContactController` actions retain their current authorization metadata.
5. The actions resolve the current principal's Microsoft token with the registered `IClaimResolver.GetMicrosoftToken` method, supplying the principal and token-unprotect delegate. They return Unauthorized without querying if the token is absent or expired.
6. `ContactsAsync` returns `Task<ActionResult<IEnumerable<ContactModel>>>`, supports both controller-root GET and `contacts` routes, executes `GetContactCollectionQuery` with the validated token, and returns mapped contacts ordered by display name using ordinal case-insensitive comparison, with an ordinal external-identifier tie-break.
7. `ContactAsync(string externalIdentifier)` supports the `{externalIdentifier}` route, applies the same validation attributes as `ContactModel.ExternalIdentifier`, and executes `GetContactQuery` with the identifier and validated token. It returns NotFound for a null query result and maps only a non-null contact.
8. `ContactsWithBirthdayAsync` accepts a required `withinDays` query parameter in the inclusive range 0–365, uses the controller-relative `withbirthdays` route, and executes `GetContactWithBirthdayCollectionQuery` with the day count and validated token. It returns mapped contacts using the same deterministic display-name ordering as `ContactsAsync`.
9. Focused WebApi.Tests verify each action's query type and arguments, mapped response, and OK result; verify NotFound for an unknown identifier; verify missing/expired-token responses do not invoke the query bus; verify a negative birthday day count is rejected; and verify birthday results use the defined ordering.
10. Focused converter tests verify the exposed contact fields and nested interface mappings. Tests also verify that `InternalIdentifier` is omitted, model validation attributes are present, new action authorization metadata is correct, and existing action authorization metadata is unchanged. Existing `ClaimResolver` tests remain the coverage for token decoding and invalid-token behavior.
11. Building the WebApi runs the existing post-build client generation and the generated client exposes all three contact operations. Generated files are not hand-edited.

## Scope

### In scope

- WebApi contact DTOs, nested models, validation, and AutoMapper mappings.
- The three read-only contact actions, their token validation, query construction, response mapping, ordering, and authorization metadata.
- Focused WebApi.Tests for action behavior, authorization metadata, model validation, and converter mappings.
- WebApi build and verification of generated client operations.

### Out of scope

- Changes to the existing contact query contracts or their Graph/repository behavior.
- Changes to MVC controllers or MVC view models, or reuse of MVC models in the WebApi.
- Changes to existing `ContactController` actions or their authorization behavior.
- Hand-editing generated WebApi.ClientApi files.

## Risks

- The controller already has a class-level `ContactPolicy`; the new action-level policy metadata must add the external-provider requirement without changing the existing actions' authorization behavior.
- The resolved token must be refreshable and all token fields must be copied into each query; checking the token before query dispatch prevents missing/expired claims from reaching contact handlers.
- Nested domain interfaces require explicit WebApi mappings; `ContactGroupModel` and `PaymentTermModel` already exist, while the other contact-valued properties need compatible WebApi models.
- The generated client is a build output and must be validated through the existing post-build process rather than edited directly.
