# Implementing WebApi logic to expose contacts

## General informations

We gona implement logic in the following projects:

* OSDevGrp.OSIntranet.WebApi
* OSDevGrp.OSIntranet.WebApi.Tests
* OSDevGrp.OSIntranet.WebApi.ClientApi which will be automatically be updated by the OSDevGrp.OSIntranet.WebApi.PostBuild

## Implement a contact controller in the WebApi

### Business goal

A new contact controller should be implemented in the WebApi project. The contact controller should be authorized with a policy ensuring the use of the bearer authenticationScheme, an authenticated user who has the following claims ClaimHelper.ContactsClaimType and MicrosoftTokenClaimType. The API version should be 1.0 and the route should be defined by "/api/[controller]".

## Implementation Bullets

### WebApi

- [ ] Add a contacts authorization policy constant to `Security/Policies.cs`.
- [ ] Register the policy in `Startup.cs` with the bearer authentication scheme, an authenticated-user requirement, and requirements for `ClaimHelper.ContactsClaimType` and `ClaimHelper.MicrosoftTokenClaimType`.
- [ ] Add a `ContactController` deriving from `ControllerBase`, decorated with `[ApiController]`, `[ApiVersion("1.0")]`, `[Route("api/[controller]")]`, and `[Authorize(Policy = ...)]` for the contacts policy.
- [ ] Keep this change limited to an empty controller and its authorization/versioning foundation; do not add placeholder actions. Define contact operations separately.

### WebApi.Tests

- [ ] Add focused tests verifying the controller's route, API version, and contacts authorization policy metadata.
- [ ] Verify the contacts policy uses the bearer authentication scheme, requires an authenticated user, and requires both specified claims.

### WebApi.ClientApi

- [ ] Build the WebApi project to regenerate the client API through its existing post-build process; do not hand-edit generated files. No contact client methods are expected until contact operations are added.
