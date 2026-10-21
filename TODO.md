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

## Expose logic to delete a posting line within a given accounting's posting journal from the BFF WebApi

### Business Goal

The WebApi should expose logic to delete an existing posting line within a given accounting's posting journal. This should support the React application to delete an existing posting line within a given accounting's posting journal then reload the posting journal.

### Implementation Bullets

#### BFF DomainServices

- [ ] Add `DeletePostingLineFromPostingJournalRequest.cs` in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/DeletePostingLineFromPostingJournal/`, inheriting from `PostingJournalLineIdentificationRequestBase` and implementing `IHumanVerifiableRequest` with the verification key and code, so the command feature pipeline performs human verification.
- [ ] Add `DeletePostingLineFromPostingJournalFeature.cs` in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/DeletePostingLineFromPostingJournal/`, inheriting from `PostingLineFeatureBase<DeletePostingLineFromPostingJournalRequest>`.
- [ ] Remove only the posting line matching the request identifier; if none matches, throw `UnknownIdentifierException` with the requested identifier and localized text from `StaticTextKey.UnknownIdentifier`.
- [ ] Because `ApplyPostingLineModel` is immutable, construct a new posting line collection containing every existing line except the one being deleted, then pass it to `SortAndClonePostingJournal` to sort, clone, and return the updated journal.

#### BFF DomainServices.Tests

- [ ] Add `Features/Commands/Accounting/DeletePostingLineFromPostingJournal/DeletePostingLineFromPostingJournalFeature/ExecuteAsyncTests.cs`, following the Append and Modify feature test conventions.
- [ ] Verify the matching posting line is removed and all other lines remain unchanged.
- [ ] Verify a missing identifier throws `UnknownIdentifierException` with the requested identifier and retrieves its message from `StaticTextKey.UnknownIdentifier`.
- [ ] Verify the updated journal is saved with the request accounting number and cancellation token, and the returned journal is cloned and sorted.

#### BFF WebApi Request Contract

- [ ] Use the required `X-Verification-Key` and `X-Verification-Code` headers for the verification key and code; keep both values out of the URL and request body, and ensure they are redacted from logs.

#### BFF WebApi Endpoint

- [ ] Add `DeletePostingLineFromPostingJournalAsync` to `AccountingController` as an authorized `DELETE` endpoint using `[HttpDelete("{accountingNumber:int}/postingjournal/postinglines/{identifier}")]`, with the standard response metadata used by the posting journal command endpoints.
- [ ] Add `[Authorize(Policy = Policies.AccountingModifier)]`.
- [ ] Add `[ProducesResponseType(typeof(PostingJournalResponseDto), (int)HttpStatusCode.OK, MediaTypeNames.Application.Json)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Add `[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.ProblemJson)]`.
- [ ] Bind and validate the accounting number and posting line identifier from the route, require the `X-Verification-Key` and `X-Verification-Code` headers, and accept a cancellation token.
- [ ] Accept `[FromServices] ICommandFeature<DeletePostingLineFromPostingJournalRequest> commandFeature` and `[FromServices] IQueryFeature<PostingJournalRequest, PostingJournalResponse> queryFeature`.
- [ ] Resolve the security context, create `DeletePostingLineFromPostingJournalRequest`, execute the delete command, then execute the query feature for the refreshed journal and return `PostingJournalResponseDto.Map` in an `OK` result.

#### BFF WebApi Tests

- [ ] Add `Controllers/Accounting/AccountingController/DeletePostingLineFromPostingJournalAsyncTests.cs`, following the Modify endpoint test conventions.
- [ ] Verify the security context is resolved once and the command receives the route values, verification key and code, format provider, security context, and cancellation token.
- [ ] Verify the refreshed journal is queried with the accounting number, current local status date, format provider, security context, and cancellation token, and the method returns `OkObjectResult` containing a `PostingJournalResponseDto`.

#### React Application

- [ ] Add `deletePostingLineFromPostingLineJournal` to `AccountingService`, following the required-value checks in the append and modify methods; send a `DELETE` request to the endpoint with `X-Verification-Key` and `X-Verification-Code` headers, use the existing antiforgery, credentials, and error-handling conventions, and return `response.json()` on success.
- [ ] Implement `handleDelete` in `PostingJournal.jsx` to convert the accounting number with `formHelper.convertToInteger` and the posting line identifier with `formHelper.convertToUuid`, then call the service with those values and the verification key and code.
- [ ] On successful deletion, close the confirmation modal and update the journal from `response.dynamicTexts`; on failure, show the same danger toast as the create handler and keep the confirmation modal open.
- [ ] Remove the temporary `console.debug` calls from the delete handler.
