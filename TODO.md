# Implementing WebApi logic to expose contacts

## General informations

We gona implement logic in the following projects:

* OSDevGrp.OSIntranet.WebApi
* OSDevGrp.OSIntranet.WebApi.Tests
* OSDevGrp.OSIntranet.WebApi.ClientApi which will be automatically be updated by the OSDevGrp.OSIntranet.WebApi.PostBuild

We gona create tests for functionality in the following projects:

* OSDevGrp.OSIntranet.WebApi.Tests

## Implement a contact controller in the WebApi

### Business goal

In the contact controller we gona expose contacts based on the following methods:

* One returning all contacts
* One returning a given contact based on the external identifier
* One returning all contacts who has birthday within a given number of days

All method should be authorized by the **ContactsFromExternalProviderPolicy** and implement logic to validate that the auhtorized user has the **MicrosoftTokenClaimType** claim containing a non expired token.

## Implementation Bullets

### WebApi

- [ ] Add `Models/Contacts/ContactModel.cs` and expose every `IContact` property except `InternalIdentifier`; do not expose its methods or reuse MVC view models.
- [ ] Represent each interface-typed `IContact` property (`Name`, `Address`, `Company`, `ContactGroup`, and `PaymentTerm`) with a separate WebApi model; reuse existing compatible models and add any missing models.
- [ ] Add the relevant validation attributes to `ContactModel` and its nested WebApi models, using `ContactViewModel` and its submodels as the validation reference without adding a dependency on MVC.
- [ ] Register the `IContact` to `ContactModel` mapping and all required interface-to-WebApi-model mappings for its interface-valued properties in `ContactModelConverter`.
- [ ] Reuse the registered `IClaimResolver.GetMicrosoftToken` method to resolve the current user's Microsoft token claim, supplying the principal and token-unprotect delegate; do not add a separate token-resolver interface, implementation, or registration.
- [ ] Return `Unauthorized` without querying when no token is resolved or the resolved token has expired.
- [ ] Add `ContactsAsync` with signature `Task<ActionResult<IEnumerable<ContactModel>>>` and both `[HttpGet]` and `[HttpGet("contacts")]` attributes; resolve the token, execute `GetContactCollectionQuery`, map the contacts to `ContactModel`, and return them in display-name order using an ordinal case-insensitive comparison, with external identifier as the tie-breaker.
- [ ] Add `ContactAsync(string externalIdentifier)` with `[HttpGet("{externalIdentifier}")]`; apply the same validation attributes to the parameter as `ContactModel.ExternalIdentifier` and execute `GetContactQuery` with the identifier and token. `GetContactQueryHandler` returns `null` when Microsoft Graph has no matching contact, so return `NotFound` for a null result and map only a non-null contact.
- [ ] Add `ContactsWithBirthdayAsync([Required][Range(0, 365)] [FromQuery] int withinDays)` with `[HttpGet("withbirthdays")]`; execute `GetContactWithBirthdayCollectionQuery` with `withinDays` and the token, map the contacts to `ContactModel`, and return them in display-name order using an ordinal case-insensitive comparison, with external identifier as the tie-breaker.
- [ ] Protect each new action with `Policies.ContactsFromExternalProviderPolicy` while preserving the existing authorization behavior of other `ContactController` actions.
- [ ] Populate each contact query's token fields from the validated refreshable token and use the existing `IQueryBus` and contact converter patterns.

### WebApi.Tests

- [ ] Add focused tests for each new action to verify its query type and arguments, mapped response, and expected `OK` result; verify `NotFound` for an unknown external identifier.
- [ ] Verify missing and expired Microsoft-token claims return `Unauthorized` and do not invoke the query bus; rely on the existing `ClaimResolver` tests for token-claim decoding and invalid-token behavior.
- [ ] Verify the birthday action rejects a negative day count and returns matching birthday contacts in deterministic display-name order.
- [ ] Verify the new actions declare `ContactsFromExternalProviderPolicy` without changing the existing controller actions' authorization metadata.
- [ ] Add focused `ContactModelConverter` tests for mapping the exposed `IContact` fields and interface-valued properties to their WebApi models.
- [ ] Verify `ContactModel` omits `InternalIdentifier` and that the WebApi models carry the relevant validation attributes from the MVC model equivalents.

### WebApi.ClientApi

- [ ] Build the WebApi project to run its existing post-build client generation and verify the generated client exposes the three contact operations; do not hand-edit generated files.
