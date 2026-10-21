# PRD: Accounting Posting Journal Updates

## Problem

Three related gaps affect accounting users and the build process:

- Failed human verification currently produces a hard-coded English exception message instead of the static-text message used by the Danish BFF experience.
- The BFF DomainServices project uses ImageSharp packages that must be upgraded, and the acquired ImageSharp license must be available to builds without entering source control, the Docker build context, a published output, or an image layer.
- Users can append and modify posting-journal lines but cannot delete one from the React application through the BFF WebApi.

## Relevant Codebase

- **What's there**
  - Human-verifiable requests currently expose only their verification key and code in [IHumanVerifiableRequest.cs:3](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Cqs/IHumanVerifiableRequest.cs#L3). [VerificationFailedException.cs:3](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/VerificationFailedException.cs#L3) uses a hard-coded English message. The command and query decorators throw that exception when verification fails: [CommandFeatureHumanVerifier.cs:7](../../OSDevGrp.OSIntranet.Bff.DomainServices/Cqs/PipelineExtensions/FeatureHumanVerifier/CommandFeatureHumanVerifier.cs#L7) and [QueryFeatureHumanVerifier.cs:7](../../OSDevGrp.OSIntranet.Bff.DomainServices/Cqs/PipelineExtensions/FeatureHumanVerifier/QueryFeatureHumanVerifier.cs#L7).
  - [StaticTextKey.cs:3](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs#L3) defines static-text keys. [StaticTextProvider.cs:16](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs#L16) resolves a key and formats its text using an `IFormatProvider`; the existing tests are data-driven in [GetStaticTextAsyncTests.cs:148](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/StaticTextProvider/GetStaticTextAsyncTests.cs#L148).
  - [OSDevGrp.OSIntranet.Bff.DomainServices.csproj:4](../../OSDevGrp.OSIntranet.Bff.DomainServices/OSDevGrp.OSIntranet.Bff.DomainServices.csproj#L4) targets `net10.0` and currently references ImageSharp `3.1.12` ([line 15](../../OSDevGrp.OSIntranet.Bff.DomainServices/OSDevGrp.OSIntranet.Bff.DomainServices.csproj#L15)) and ImageSharp.Drawing `2.1.7` ([line 16](../../OSDevGrp.OSIntranet.Bff.DomainServices/OSDevGrp.OSIntranet.Bff.DomainServices.csproj#L16)). [CaptchaGenerator.cs:12](../../OSDevGrp.OSIntranet.Bff.DomainServices/Security/CaptchaGenerator.cs#L12) uses both packages.
  - The root [.gitignore:252](../../.gitignore#L252) already ignores the `Licenses/` directory. [`.dockerignore`](../../.dockerignore) currently does not exclude it. The `builder` service in [docker-compose.yaml:3](../../docker-compose.yaml#L3) has no build secret configured. [OSDevGrp.OSIntranet.Builder.Dockerfile:57](../../OSDevGrp.OSIntranet.Builder.Dockerfile#L57) publishes the BFF WebApi, which builds BFF DomainServices; [OSDevGrp.OSIntranet.Bff.WebApi.Dockerfile:1](../../OSDevGrp.OSIntranet.Bff.WebApi.Dockerfile#L1) copies the published output into the runtime image.
  - Posting-line commands inherit from [PostingLineFeatureBase.cs:8](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs#L8), which loads and saves the posting journal and provides sorting/cloning at [line 46](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs#L46). [ModifyPostingLineInPostingJournalFeature.cs:33](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/ModifyPostingLineInPostingJournal/ModifyPostingLineInPostingJournalFeature.cs#L33) demonstrates identifier lookup, localized `UnknownIdentifierException` handling, and returning the sorted/cloned journal.
  - [AccountingController.cs:212](../../OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs#L212) contains the authorized modify endpoint and its command-then-query refresh pattern. The refresh uses the controller's local status date at [line 244](../../OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs#L244).
  - [AccountingService.jsx:119](../../osdevgrp.osintranet.react/src/services/AccountingService.jsx#L119) and [line 186](../../osdevgrp.osintranet.react/src/services/AccountingService.jsx#L186) show the append/modify service conventions. [PostingJournal.jsx:576](../../osdevgrp.osintranet.react/src/components/PostingJournal.jsx#L576) currently logs delete context and verification credentials and closes the confirmation immediately. [DeleteConfirmation.jsx:71](../../osdevgrp.osintranet.react/src/components/DeleteConfirmation.jsx#L71) invokes the page's delete handler.

- **How it works**
  - Command/query feature decorators validate verification credentials before invoking the inner feature and remove the verification code afterward. On a failed verification, the exception message is returned as a bad-request problem detail by the BFF error handling.
  - Static texts are stored in the DomainServices provider and resolved with the request's format provider. Posting-line identification requests already carry a format provider in [PostingJournalLineIdentificationRequestBase.cs:22](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingJournalLineIdentificationRequestBase.cs#L22).
  - The existing posting-line API endpoints resolve one security context, execute a command, query the refreshed journal, and return a mapped response. The React service methods use credentials, antiforgery headers, and shared error generation; the page updates its journal from `response.dynamicTexts` on successful create or modify.
  - The builder image publishes several applications in separate Dockerfile steps. The BFF publish step is the step that must receive the ImageSharp license secret; the runtime image is based on the builder output.

- **Patterns to follow**
  - Resolve user-facing errors via `IStaticTextProvider`, passing the request's `IFormatProvider`, and test both the selected key and localized text.
  - Follow the append/modify posting-line command and controller patterns, and use `SortAndClonePostingJournal` for the updated journal.
  - Follow existing ASP.NET authorization, route validation, response metadata, security-context, cancellation-token, and response-DTO conventions.
  - Follow the React accounting service's required-value, antiforgery, credentials, and error-handling conventions. The delete confirmation remains open after a failed request and closes only after a successful response.
  - Keep the acquired license file out of tracked source and Docker contexts; use a BuildKit secret only for the BFF publish operation.

- **Integration points**
  - The deletion request is human-verifiable and participates in the existing command pipeline. The controller sends the accounting number, route identifier, verification key/code, format provider, security context, and cancellation token to the command, then queries the refreshed journal.
  - The React page calls the new `AccountingService` delete method from its confirmation handler and replaces its displayed journal with the returned dynamic texts after success.
  - The builder Dockerfile's BFF publish operation consumes the Compose-provided license secret. The BFF runtime image receives only published application output.

## Goal

Accounting users receive a Danish verification-failure message and can delete a posting line from an accounting's journal through the authorized BFF WebApi and React application. The BFF DomainServices build uses ImageSharp 4-compatible packages and consumes the acquired license securely in local and Docker builds without publishing or packaging the license.

## User Stories

- As an accounting user, I want a failed verification to be explained in Danish so that the error is understandable in the application's language.
- As an authorized accounting modifier, I want to delete a posting line and see the refreshed journal so that I can correct the accounting record.
- As an authorized accounting modifier, I want a failed deletion to leave the confirmation available and show an error so that I can understand the failure and retry or cancel.
- As a developer building the application, I want the required ImageSharp license supplied securely to local and Docker builds so that builds succeed without exposing the license in source control, Docker contexts, images, or published output.

## Acceptance Criteria

1. `StaticTextKey.VerificationFailed` exists, and the static-text provider resolves it to `Den angivne bekræftelseskode kunne ikke godkendes.`.
2. `VerificationFailedException` accepts the resolved message rather than embedding an English message. Both human-verifier decorators use `IStaticTextProvider` and the human-verifiable request's `FormatProvider` when verification fails, then throw the exception with the localized text.
3. DomainServices tests cover the new static-text key and verify that both decorators resolve and pass the localized message when verification fails.
4. BFF DomainServices references ImageSharp `4.1.2` and an ImageSharp.Drawing version compatible with ImageSharp 4 (the TODO names `3.1.2` as an example). The selected versions support `net10.0`, and `CaptchaGenerator` compiles and retains its expected behavior.
5. The project configures `SixLaborsLicenseFile` to the repository-root `licenses/sixlabors.lic` for local builds. The existing Git ignore rule remains in place, and `/licenses/` is excluded from the Docker build context.
6. Compose declares the `sixlabors_license` file secret sourced from `./licenses/sixlabors.lic` and attaches it to the `builder` service's build. The builder Dockerfile mounts that BuildKit secret only for the BFF WebApi publish step and passes `/run/secrets/sixlabors.lic` through `-p:SixLaborsLicenseFile=...`.
7. The builder setup uses a BuildKit-capable builder, and a concise comment in the builder Dockerfile identifies `docker compose build builder` as the command for building with the license secret.
8. Local BFF DomainServices builds find the license through the project-relative property. `docker compose build builder` successfully publishes the BFF WebApi using the secret, and the license file is absent from both publish output and the resulting image.
9. The delete request inherits from `PostingJournalLineIdentificationRequestBase`, carries the verification key and code, and implements `IHumanVerifiableRequest`, allowing the existing command pipeline to perform human verification.
10. The delete feature removes only the line matching the requested identifier. If no line matches, it throws `UnknownIdentifierException` containing the requested identifier and text resolved through `StaticTextKey.UnknownIdentifier`. It creates a collection containing all remaining lines and passes it to `SortAndClonePostingJournal`; the updated journal is saved with the request accounting number and cancellation token.
11. DomainServices unit tests verify deletion and preservation of the other lines, the missing-identifier exception and static-text lookup, saving with the request accounting number and cancellation token, and the sorted/cloned returned journal.
12. `AccountingController` exposes the authorized `DELETE` route `"{accountingNumber:int}/postingjournal/postinglines/{identifier}"` under `api/accounting`, with the specified response metadata. It validates route values, requires `X-Verification-Key` and `X-Verification-Code` headers, accepts a cancellation token, resolves the security context once, executes the command, then queries the refreshed journal using the accounting number, current local status date, format provider, security context, and cancellation token, returning an `OK` result containing `PostingJournalResponseDto`.
13. Verification key and code are never placed in the URL or request body and are never logged. The React delete handler has no debug logging of the verification values.
14. BFF WebApi tests verify the security-context resolution, command request values and cancellation token, refreshed-journal query values, and mapped `OkObjectResult`.
15. `AccountingService` exposes `deletePostingLineFromPostingLineJournal`, validates required values, sends a `DELETE` request with the verification headers plus existing antiforgery and credential behavior, uses the established error handling, and returns parsed JSON on success.
16. `PostingJournal.jsx` converts the accounting number and identifier using the existing form helpers before calling the service. On success it closes the confirmation and updates the journal from `response.dynamicTexts`; on failure it displays the create handler's danger toast and keeps the confirmation open.
17. Focused `CaptchaGenerator` tests and BFF DomainServices unit tests pass, and the BFF WebApi tests for the new endpoint pass. React is validated manually; automated React tests are not required.

## Scope

### In scope

- Localizing failed human-verification messages in BFF DomainServices.Interfaces, BFF DomainServices, and BFF DomainServices.Tests.
- Upgrading ImageSharp packages, adapting `CaptchaGenerator` as needed, and configuring local and Docker license handling and build verification.
- Adding the posting-line deletion request and feature, their DomainServices tests, the authorized BFF WebApi endpoint and tests, and the React service/page behavior.
- Adding a short build-command comment to the builder Dockerfile.

### Out of scope

- `OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces`, `OSDevGrp.OSIntranet.Bff.ServiceGateways`, `OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests`, and `OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData`.
- Automated tests for the React application.
- Separate documentation for the Docker Compose build command.

## Risks

- Docker builds require a BuildKit-capable builder and correct scoping of the license secret to the BFF publish step. The secret must not be copied into an image layer or publish directory.
- Local and container build verification depends on the acquired license being available at the specified path; its contents must not be exposed.
- The ImageSharp major-version upgrade may require API adjustments in `CaptchaGenerator`; package compatibility and existing CAPTCHA behavior must both be checked.
- The verification headers contain sensitive values. They must be excluded from browser and application logging as well as from the URL and body.
- The delete command must preserve all non-matching immutable posting-line models and the established journal sort/clone behavior.
