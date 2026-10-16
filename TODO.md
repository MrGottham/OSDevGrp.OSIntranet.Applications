# Appending posting journal for an accounting

## General

We need to append/implement functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* OSDevGrp.OSIntranet.Bff.WebApi
* osdevgrp.osintranet.react

We need to create tests for functionality in the following projects:

* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* **OUT OF SCOPE:** OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* OSDevGrp.OSIntranet.Bff.WebApi.Tests

Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Resolve verification failure messages through static text

### Business Goal

Verification failures should expose a Danish message resolved through the static text provider instead of a hard-coded English exception message.

### Implementation Bullets

#### BFF DomainServices.Interfaces

- [ ] Add `VerificationFailed` to `StaticTextKey`.
- [ ] Update `VerificationFailedException` to accept the localized message instead of using a hard-coded English message.
- [ ] Add `IFormatProvider FormatProvider { get; }` property to `IHumanVerifiableRequest` to enable culture-aware error message localization.

#### BFF DomainServices

- [ ] Add the Danish text `Den angivne bekræftelseskode kunne ikke godkendes.` for `StaticTextKey.VerificationFailed` in `StaticTextProvider`.
- [ ] Update `CommandFeatureHumanVerifier` and `QueryFeatureHumanVerifier` to resolve the message through `IStaticTextProvider` using `request.FormatProvider` when verification fails, then throw `VerificationFailedException` with that localized message.

#### BFF DomainServices.Tests

- [ ] Add `[TestCase(StaticTextKey.VerificationFailed, "Den angivne bekræftelseskode kunne ikke godkendes.", 0)]` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Verify both human-verifier decorators resolve the new key and pass the localized message to `VerificationFailedException` when verification fails.

## Upgrade SixLabors ImageSharp packages and configure build licensing

### BFF DomainServices

- [ ] Upgrade the direct `SixLabors.ImageSharp` reference in `OSDevGrp.OSIntranet.Bff.DomainServices.csproj` from `3.1.12` to `4.1.2`.
- [ ] Upgrade the direct `SixLabors.ImageSharp.Drawing` reference from `2.1.7` to a version compatible with ImageSharp 4, such as `3.1.2`.
- [ ] Review and update `CaptchaGenerator` for API changes in ImageSharp 4 and ImageSharp.Drawing 3.
- [ ] Confirm the selected package versions support the project's `net10.0` target.

### Licensing and Build Configuration

- [ ] Keep the acquired license file at `licenses/sixlabors.lic` in the solution root; retain the `.gitignore` rule so the license is not committed.
- [ ] Add `SixLaborsLicenseFile` to the `BFF.DomainServices.csproj` property group using `$(MSBuildThisFileDirectory)../licenses/sixlabors.lic` for local builds.
- [ ] Add `/licenses/` to `.dockerignore` so the license is not copied into the Docker build context by `COPY . .`.
- [ ] Declare a top-level `sixlabors_license` file secret in `docker-compose.yaml`, sourced from `./licenses/sixlabors.lic`, and attach it to the `builder` service's build configuration.
- [ ] Mount the `sixlabors_license` BuildKit secret only for the `dotnet publish` step that builds BFF DomainServices, and pass `/run/secrets/sixlabors.lic` through `-p:SixLaborsLicenseFile=...` to override the project-relative path in the container.
- [ ] Ensure Docker Compose uses a BuildKit-capable builder and document `docker compose build builder` as the command for building with the license secret.
- [ ] Ensure the license file is not copied into a Docker image layer or included in published application output.

### BFF DomainServices.Tests

- [ ] Run the focused `CaptchaGenerator` tests and update them if the package API changes affect expected behavior.
- [ ] Run the BFF DomainServices unit tests and confirm the project builds with the license configuration.

### Build Verification

- [ ] Verify local builds find the license using the project-relative `SixLaborsLicenseFile` path.
- [ ] Verify `docker compose build builder` successfully publishes the BFF WebApi with the BuildKit license secret, and confirm the license file is absent from the resulting image and publish output.

## Expose logic to modify a posting line within a given accounting's posting journal from the BFF WebApi

### Business Goal

The WebApi should expose logic to modify an existing posting line within a given accounting's posting journal. This should support the React application to modify an existing posting line within a given accounting's posting journal then reload the posting journal.

### Implementation Bullets

#### BFF DomainServices

- [ ] Create `Features/Commands/Accounting/ModifyPostingLineInPostingJournal/ModifyPostingLineInPostingJournalRequest.cs` with namespace `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting` and inherit from `PostingJournalLineDataRequestBase`.
- [ ] Create `Features/Commands/Accounting/ModifyPostingLineInPostingJournal/ModifyPostingLineInPostingJournalFeature.cs` with namespace `OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting`, inheriting from `PostingLineFeatureBase<ModifyPostingLineInPostingJournalRequest>`.
- [ ] Find the existing posting line by the request identifier.
- [ ] If no line matches, throw `UnknownIdentifierException` with the request identifier and localized text from `StaticTextKey.UnknownIdentifier`.
- [ ] Since generated `ApplyPostingLineModel` properties are immutable, construct a replacement line with request values, preserving the matched line's identifier and `SortOrder`.
- [ ] Replace only the matched line in the collection; leave other lines unchanged and return the cloned journal through `SortAndClonePostingJournal`.

#### BFF DomainServices Tests

- [ ] Add `Features/Commands/Accounting/ModifyPostingLineInPostingJournal/ModifyPostingLineInPostingJournalFeature/ExecuteAsyncTests.cs` in namespace `OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.ModifyPostingLineInPostingJournal.ModifyPostingLineInPostingJournalFeature`, following the fixture and mock conventions of the Append feature's `ExecuteAsyncTests`.
- [ ] Verify a matching line is replaced with all request fields while retaining its identifier and `SortOrder`.
- [ ] Verify a missing identifier throws `UnknownIdentifierException`, includes the requested identifier, and retrieves the message from `StaticTextKey.UnknownIdentifier`.
- [ ] Verify other lines remain unchanged and the saved journal is cloned, sorted by posting date and sort order, and retains its accounting number.
- [ ] Verify `SavePostingJournalAsync` is called with the updated journal and the request accounting number/cancellation token.

#### BFF WebApi DTO

- [ ] Create `OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/ModifyPostingLineInPostingJournalDto.cs` in namespace `OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos`, inheriting from `PostingJournalLineModifierDtoBase` with no additional properties.

#### BFF WebApi Endpoint

- [ ] Add `ModifyPostingLineInPostingJournalAsync` to `AccountingController` as an authorized `PUT` endpoint using `[HttpPut("{accountingNumber:int}/postingjournal/postinglines/{identifier}")]`.
- [ ] Add `[Authorize(Policy = Policies.AccountingModifier)]`.
- [ ] Add `[ProducesResponseType(typeof(PostingJournalResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Accept `[FromServices] ICommandFeature<ModifyPostingLineInPostingJournalRequest> commandFeature` and `[FromServices] IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature`.
- [ ] Bind and validate `accountingNumber` from the route, bind the required GUID `identifier` from the route, bind `ModifyPostingLineInPostingJournalDto` from the body, and accept a `CancellationToken`.
- [ ] Resolve the security context and create `ModifyPostingLineInPostingJournalRequest` with the route values, DTO fields, and security context.
- [ ] Execute the modify command, query the refreshed journal with the accounting number, current status date, format provider, and security context, then return `PostingJournalResponseDto.Map` in an `OK` result.

#### BFF WebApi Tests

- [ ] Add `Controllers/Accounting/AccountingController/ModifyPostingLineInPostingJournalAsyncTests.cs` in namespace `OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.AccountingController`, following `AppendPostingLineToPostingJournalAsyncTests` fixture and mock conventions.
- [ ] Verify `GetCurrentSecurityContextAsync` is called once with the given cancellation token.
- [ ] Verify the command feature is called exactly once with a `ModifyPostingLineInPostingJournalRequest` whose:
  - [ ] `RequestId` is not `Guid.Empty`.
  - [ ] `AccountingNumber` matches the route value.
  - [ ] `Identifier` matches the required GUID from the route.
  - [ ] `PostingDate` matches the DTO value.
  - [ ] `PostingReference` matches the DTO value.
  - [ ] `Account` matches the DTO value.
  - [ ] `PostingText` matches the DTO value.
  - [ ] `BudgetAccount` matches the DTO value.
  - [ ] `Debit` matches the DTO value.
  - [ ] `Credit` matches the DTO value.
  - [ ] `ContactAccount` matches the DTO value.
  - [ ] `SecurityContext` is the context resolved by the security context provider.
- [ ] Verify the query feature is called once with a non-empty request ID, accounting number, current local status date, format provider, resolved security context, and cancellation token.
- [ ] Verify the method returns `OkObjectResult` containing a `PostingJournalResponseDto`.

#### React Application

- [ ] Add `modifyPostingLineInPostingLineJournal` to `AccountingService`, following the Append method's required-value checks, request headers and credentials, and error handling; send a `PUT` request to the modify endpoint with the modifier fields as JSON and return `response.json()` on success.
- [ ] Implement `handleUpdatePostingJournalLine` in `PostingJournal.jsx` using the same field conversions as the create handler: integer accounting number, UUID line identifier, ISO posting date, nullable strings for posting reference/budget/contact account, strings for account and posting text, and nullable decimals for debit and credit.
- [ ] On successful modification, close the edit modal and update the journal from `response.dynamicTexts`; on failure, show the same danger toast as the create handler and keep the modal open.
- [ ] Remove temporary `console.debug` calls from the update handler.

## Expose logic to delete a posting line within a given accounting's posting journal from the BFF WebApi

### Business Goal

The WebApi should expose logic to delete an existing posting line within a given accounting's posting journal. This should support the React application to delete an existing posting line within a given accounting's posting journal then reload the posting journal.
