# Diary: Expose contacts through the WebApi

Goal: give the WebApi a stable, versioned, authorized `api/Contact` controller foundation (PRD: `tasks/2026-11-06-expose-contacts-webapi/PRD.md`). No contact operations yet.

## Step 1: Policy, registration, empty controller, and tests

**Author:** main

### Prompt Context

**Verbatim prompt:** "yes lets go for the implementation"
**Interpretation:** Implement option 1 from the proposed iterations (whole PRD as one slice) following the agreed plan.
**Inferred intent:** Land the contacts controller foundation in one reviewable change that keeps main green.

### What I did
- `OSDevGrp.OSIntranet.WebApi/Security/Policies.cs`: added `ContactPolicy = "Contacts"`.
- `OSDevGrp.OSIntranet.WebApi/Startup.cs`: registered `ContactPolicy` in `AddAuthorization` with the bearer scheme, `RequireAuthenticatedUser`, `ContactsClaimType`, and `MicrosoftTokenClaimType`.
- `OSDevGrp.OSIntranet.WebApi/Controllers/ContactController.cs`: empty controller with `[Authorize]`, `[ApiController]`, `[ApiVersion("1.0")]`, and `[Route("api/[controller]")]`.
- `OSDevGrp.OSIntranet.WebApi.Tests/Controllers/ContactController/ContactControllerMetadataTests.cs`: six reflection tests, one assertion each.
- `OSDevGrp.OSIntranet.WebApi.Tests/Startup/ContactPolicyRegistrationTests.cs`: four tests that build the policy through `Startup.ConfigureServices` and check the scheme, authenticated-user requirement, and both claims.

### Why
The PRD's acceptance criteria require the controller metadata and the WebApi policy registration to be verified. Splitting the change would leave a controller referencing a missing policy, so the slice ships as one unit.

### What worked
- `Startup.ConfigureServices` is public, so the registration could be tested without a production seam.
- Building `Startup` with an empty `ConfigurationBuilder` was enough for the registration tests.
- Full `Category=UnitTest` run for WebApi.Tests: 1240 passed, 0 failed.
- The generated client (`OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs`) did not change.

### What didn't work
- Building `OSDevGrp.OSIntranet.WebApi.csproj` on its own failed in the post-build step:
  `No arguments matching: (--solutionDir){1}\s([A-Za-z0-9/\/.]+){1}` (MSB3073, exit 134).
  Cause: `$(SolutionDir)` is empty when a project is built directly, and `WebApi.csproj` passes `--solutionDir $(SolutionDir)`.
  Workaround: `dotnet build ... -p:SolutionDir=<repo root>/` and the same flag on `dotnet test`.

### What I learned
- The post-build client generation runs on every build and test build of WebApi, so test commands for WebApi.Tests also need `-p:SolutionDir=...` when run outside the solution build.
- A test namespace named `...WebApi.Tests.Startup` shadows the `Startup` class name inside that namespace. The registration test aliases it as `WebApiStartup`.

### What was tricky
- Each registration test rebuilds the service provider. This is slower than necessary but keeps each test to one assertion.
- The test folder `Controllers/ContactController` shares its name with the `ContactController` class. Use the `Controller` alias, as the existing `CommonController` tests do.

### What warrants review
- `Startup.cs` policy block: confirm it matches the bearer/claim pattern of the neighbouring policies.
- `ContactController.cs`: empty by design, with no actions or placeholder methods.
- Run `dotnet test ... -p:SolutionDir=<root>/ --filter "Category=UnitTest"` and confirm the new tests pass.

### Future work
- Contact operations (queries/commands, DTOs, and actions) in a later slice. Those will add client methods to the generated WebApi client through the post-build step.

## Step 2: Split the contacts policy from the external-provider policy

**Author:** main

### Prompt Context

**Verbatim prompt:** "At the @ContactController.cs we specified the policy #sym:ContactPolicy which is correct in our business. After doing some research we figured out that this policy should not required the #sym:MicrosoftTokenClaimType. Some methods we gona implement in the @CommonController.cs gona require that claim. Therefore we think we should make an extra policy named ContactsFromExternalProvider with the same claims as #sym:ContactPolicy but this one need the #sym:MicrosoftTokenClaimType as well. We should also update the @TODO.md and @PRD.md with this information."
**Interpretation:** `ContactPolicy` requires only the contacts claim. A new `ContactsFromExternalProvider` policy adds the Microsoft-token claim for future `CommonController` methods.
**Inferred intent:** Keep `ContactController` on the narrower policy and prepare a separate policy for methods that need the external provider token.

### What I did
- `OSDevGrp.OSIntranet.WebApi/Security/Policies.cs`: added `ContactsFromExternalProviderPolicy = "ContactsFromExternalProvider"`.
- `OSDevGrp.OSIntranet.WebApi/Startup.cs`: removed `MicrosoftTokenClaimType` from `ContactPolicy` and registered `ContactsFromExternalProviderPolicy` with both claims.
- `OSDevGrp.OSIntranet.WebApi.Tests/Startup/ContactPolicyRegistrationTests.cs`: replaced the Microsoft-claim test with `DoesNotRequireMicrosoftTokenClaimType`.
- `OSDevGrp.OSIntranet.WebApi.Tests/Startup/ContactsFromExternalProviderPolicyRegistrationTests.cs` (new): four single-assertion tests.
- `tasks/2026-11-06-expose-contacts-webapi/PRD.md` and `TODO.md`: updated for the split.

### What worked
- The contacts-policy and controller tests together: 14 of 14 passed.

### What warrants review
- `Startup.cs`: the external-provider policy must keep the same bearer and authenticated-user requirements as `ContactPolicy`.
- `CommonController` does not use the new policy yet. That is intentional and belongs to a later slice.
