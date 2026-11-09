# PRD: Expose contacts through the WebApi

## Problem

The WebApi does not yet have a contacts controller or a dedicated policy for it. A versioned, authorized controller foundation is needed before contact operations can be exposed through the WebApi.

## Relevant Codebase

- **What's there**:
  - `OSDevGrp.OSIntranet.WebApi/Security/Policies.cs:7-11` contains the existing WebApi policy constants.
  - `OSDevGrp.OSIntranet.WebApi/Startup.cs:112-118` configures API versioning, including version 1.0 as the default. `Startup.cs:182-212` registers authorization policies, and `Startup.cs:440` defines the bearer scheme helper.
  - `OSDevGrp.OSIntranet.WebApi/Controllers/CommonController.cs:18-21` demonstrates the WebApi controller attributes for authorization, API controller behavior, version 1.0, and the `api/[controller]` route.
  - `OSDevGrp.OSIntranet.Domain/Security/ClaimHelper.cs:19,31` defines `MicrosoftTokenClaimType` and `ContactsClaimType`.
  - `OSDevGrp.OSIntranet.Mvc/Security/Policies.cs:7` already defines `ContactPolicy` with the value `Contacts`. Its registration in `OSDevGrp.OSIntranet.Mvc/Startup.cs:197-202` uses the MVC internal scheme and only the contacts claim, so the WebApi policies must use their own bearer scheme.
  - `OSDevGrp.OSIntranet.WebApi.Tests/Controllers/CommonController/LetterHeadsAsyncTests.cs` demonstrates the WebApi.Tests NUnit style. The WebApi test project references the WebApi project.
  - `OSDevGrp.OSIntranet.WebApi/OSDevGrp.OSIntranet.WebApi.csproj:39-46` invokes the post-build process that regenerates the client API.
- **How it works**: WebApi controller attributes select the named authorization policy and declare route/version metadata. Startup registers each policy with an authentication scheme, an authenticated-user requirement, and claim requirements. Building WebApi runs the existing post-build client generation process.
- **Patterns to follow**: Use the existing `Policies` constants and `Startup.AddAuthorization` registrations; match the controller metadata in `CommonController`; use focused NUnit tests in WebApi.Tests. Do not hand-edit generated client files.
- **Integration points**: The controller's policy depends on the shared `ClaimHelper` claim types and the WebApi bearer authentication scheme. The WebApi post-build process generates the WebApi.ClientApi output from the API document.

## Goal

Provide a stable, versioned WebApi contacts route protected by bearer authentication and the contacts claim, plus a separate `ContactsFromExternalProvider` policy that additionally requires the Microsoft-token claim for operations that need an external provider token. No contact operations are added in this slice.

## User Stories

- As a future contacts API consumer, I want a stable, authorized version-1 contacts controller foundation so that contact operations can be exposed through the WebApi in a separate change.

## Acceptance Criteria

1. `OSDevGrp.OSIntranet.WebApi/Security/Policies.cs` defines `ContactPolicy` with the value `Contacts`.
2. `Startup.cs` registers `ContactPolicy` with the bearer authentication scheme, requires an authenticated user, and requires `ClaimHelper.ContactsClaimType` only.
2a. `Policies.cs` defines `ContactsFromExternalProviderPolicy` with the value `ContactsFromExternalProvider`. `Startup.cs` registers it with the bearer authentication scheme, requires an authenticated user, and requires both `ClaimHelper.ContactsClaimType` and `ClaimHelper.MicrosoftTokenClaimType`.
3. An empty `ContactController` derives from `ControllerBase` and has `[ApiController]`, `[ApiVersion("1.0")]`, `[Route("api/[controller]")]`, and `[Authorize(Policy = Policies.ContactPolicy)]` metadata. It has no placeholder actions.
4. Focused WebApi.Tests verify the controller's route, API version, and authorization policy metadata, and verify that `ContactPolicy` uses the bearer scheme, requires authentication, and requires only the contacts claim. They also verify that `ContactsFromExternalProviderPolicy` uses the bearer scheme, requires authentication, and requires both claims. Each test asserts one behavior.
5. Building the WebApi runs its existing post-build client API generation process. Generated files are not hand-edited; no contact client methods are expected until contact operations are added.

## Scope

### In scope

- The WebApi contact policy constant and registration.
- An additional `ContactsFromExternalProviderPolicy` constant and registration for operations that need the Microsoft-token claim.
- An empty, secured, version-1 `ContactController`.
- Focused tests for the controller metadata and policy requirements.
- Running the existing WebApi build/post-build generation process.

### Out of scope

- Contact endpoints, placeholder actions, or contact operations.
- Hand-editing generated WebApi.ClientApi files.

## Risks

- With no controller actions, client generation should not produce contact client methods; this is expected rather than a failure.
- The MVC `ContactPolicy` has the same policy name but different authentication and claim requirements. The WebApi `ContactPolicy` must use the bearer scheme and only the contacts claim. The Microsoft-token requirement belongs to `ContactsFromExternalProviderPolicy`.
