# PRD: Posting Journal Line Command Foundation

## Problem

Future work will need to expose logic for adding, updating, and removing individual posting lines on a given accounting's posting journal from the BFF WebApi. Before any of those concrete commands can be built, the BFF `DomainServices` layer needs a shared foundation: reusable request/feature base classes for posting-journal-line commands, and a validation exception hierarchy that lets those commands signal identifier-related failures (e.g. a client-supplied identifier that already exists, or one that doesn't exist) in a way the WebApi layer can map to clean HTTP responses.

Additionally, an existing bug in the WebApi's error-handling pipeline maps the wrong `VerificationException` type (the framework's `System.Security.VerificationException` instead of the domain's `VerificationFailedException`), which should be fixed as part of this same error-handling pass.

This PRD covers only the foundation — not the concrete Add/Update/Delete posting-line features, and not any WebApi controller endpoint. Those will be scoped in future PRDs that build on top of this foundation.

## Relevant Codebase

**What's there:**

- [AccountingIdentificationRequestBase.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationRequestBase.cs) — abstract command request base, inherits `RequestBase`, carries `AccountingNumber`. This is the direct parent for the new `PostingJournalLineIdentificationRequestBase`.
- [AccountingIdentificationFeatureBase.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationFeatureBase.cs) — abstract command feature base, generic over `TAccountingIdentificationRequest : AccountingIdentificationRequestBase`. Implements `ICommandFeature<T>` and `IPermissionVerifiable<T>`; exposes protected `PermissionChecker` and `AccountingGateway` properties; `VerifyPermissionAsync` checks `IsAuthenticated` → `HasAccountingAccess` → `IsAccountingModifier`. This is the direct parent for `PostingLineFeatureBase`.
- [ICommandFeature.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Cqs/ICommandFeature.cs) — `Task ExecuteAsync(TRequest, CancellationToken)`, no return value.
- [IAccountingGateway.cs](../../OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs) — already exposes `GetPostingJournalAsync(int, CancellationToken)` and `SavePostingJournalAsync(int, ApplyPostingJournalModel, CancellationToken)`, both using the NSwag-generated `ApplyPostingJournalModel` (`OSDevGrp.OSIntranet.WebApi.ClientApi`), which future concrete features will use to read and persist posting journal state.
- [PostingLineDisplayerDto.cs](../../OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingLineDisplayerDto.cs) — existing WebApi-facing DTO with the canonical field naming to follow: `PostingDate`, `PostingReference`, `Account`, `PostingText`, `BudgetAccount`, `Debit`, `Credit`, `ContactAccount`. Does **not** include `SortOrder`.
- [VerificationFailedException.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/VerificationFailedException.cs) — existing domain exception; establishes the plain-`Exception`-subclass pattern used for domain validation failures.
- [ProblemDetailsFactory.cs](../../OSDevGrp.OSIntranet.Bff.WebApi/Filters/ErrorHandling/ProblemDetailsFactory.cs) — maps exception types to `ProblemDetails` via a `Dictionary<Type, Func<HttpRequest, Exception, ProblemDetails>>`. Currently maps `System.Security.VerificationException` (framework type) where it should map `VerificationFailedException` (domain type) — a pre-existing bug to fix here.
- [StaticTextKey.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Logic/StaticText/StaticTextKey.cs) — already has `PostingJournalLineIdentifier`, `AddPostingJournalLine`, `UpdatePostingJournalLine`, `DeletePostingJournalLine` keys, confirming Add/Update/Delete are anticipated future siblings sharing this foundation.
- [StaticTextProvider.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs) / [GetStaticTextAsyncTests.cs](../../OSDevGrp.OSIntranet.Bff.DomainServices.Tests/Logic/StaticText/StaticTextProvider/GetStaticTextAsyncTests.cs) — established pattern for adding a Danish translation plus a parametrized `[TestCase]`.
- Prior art: [tasks/2026-09-09-accounting-command-base-classes/PRD.md](../2026-09-09-accounting-command-base-classes/PRD.md) — the immediate predecessor PRD that built `AccountingIdentificationRequestBase`/`AccountingIdentificationFeatureBase` as foundation ahead of any concrete command, establishing the same foundation-first approach this PRD continues.

**How it works:** A concrete future command feature would inherit `PostingLineFeatureBase<TRequest>`, which in turn inherits `AccountingIdentificationFeatureBase<TRequest>` for permission-checking and gateway access. The base's `ExecuteAsync` will fetch the current posting journal via the gateway, delegate to an abstract `ProcessPostingJournalAsync` for the subclass-specific mutation (add/update/delete a line), then persist the result via the gateway. `ProcessPostingJournalAsync` returns `Task` (not `Task<ApplyPostingJournalModel>`) and mutates the fetched `ApplyPostingJournalModel` instance in place (e.g. adding to its `ApplyPostingLines` collection) — the exact same object instance flows from the gateway's `GetPostingJournalAsync` result, through `ProcessPostingJournalAsync`, to the `SavePostingJournalAsync` call; it is never replaced with a new instance. If a subclass detects an identifier conflict (adding a line whose identifier already exists) or a missing identifier (updating/deleting a line that doesn't exist), it throws one of the new exceptions, which `ProblemDetailsFactory` maps to a 400 Bad Request.

**Patterns to follow:**
- `#region` blocks: `Constructor`, `Properties`, `Methods` (mandatory per repo convention).
- Constructor injection with `?? throw new ArgumentNullException(...)` guard style as seen in `AccountingIdentificationFeatureBase`.
- Protected get-only properties exposing injected dependencies to derived classes (`PermissionChecker`, `AccountingGateway` pattern).
- Field naming aligned with `PostingLineDisplayerDto`, not the raw NSwag wire model (`ApplyPostingLineModel` uses `AccountNumber`/`Details`/`double?`; the BFF-facing naming uses `Account`/`PostingText`/`decimal?`).

**Integration points:**
- `ProblemDetailsFactory` is the sole integration point between the new exceptions and WebApi HTTP responses.
- `IStaticTextProvider` is the integration point for exposing new error message texts.
- Future concrete Add/Update/Delete features will be the actual callers of `PostingLineFeatureBase` and throwers of the new exceptions — out of scope here, but this foundation must support them without rework.

## Goal

The following exist, are fully unit-tested, and compile cleanly, without any concrete Add/Update/Delete posting-line feature or WebApi endpoint being built yet:

1. A request base class hierarchy for posting-journal-line commands, carrying an `Identifier` and the line's data fields.
2. A command feature base class that fetches, delegates processing of, and saves a posting journal, for reuse by future concrete commands.
3. A validation exception hierarchy that lets future commands signal identifier conflicts.
4. `ProblemDetailsFactory` correctly maps `VerificationFailedException` (fixing the existing bug) and the two new identifier exceptions to 400 Bad Request responses.
5. Static text entries (with Danish translations) exist for the two new exceptions' user-facing messages.

## User Stories

- As a developer building the future "add posting line" command, I want a reusable request base and feature base so that I don't duplicate the permission-checking, gateway-fetch, and gateway-save boilerplate already proven in `AccountingIdentificationFeatureBase`.
- As a developer building future "update" and "delete" posting-line commands, I want a shared `IdentifierExceptionBase` hierarchy so that identifier-not-found and identifier-already-exists failures are handled consistently across all three commands.
- As an API consumer of the future posting-line endpoints, I want identifier-related failures to come back as clear 400 Bad Request responses with a descriptive message, not as unhandled 500 errors.
- As a user of the application, I want error messages about posting-line identifiers to be shown in Danish, consistent with the rest of the application's static text.

## Acceptance Criteria

1. `PostingJournalLineIdentificationRequestBase` exists in `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/`, is `public abstract`, inherits `AccountingIdentificationRequestBase`, and adds a get-only `Identifier` (`Guid`) property set via a protected constructor accepting `requestId`, `accountingNumber`, `identifier`, `securityContext`.
2. `PostingJournalLineDataRequestBase` exists in the same folder, is `public abstract`, inherits `PostingJournalLineIdentificationRequestBase`, and adds get-only properties `PostingDate` (`DateTimeOffset`), `PostingReference` (`string?`), `Account` (`string`), `PostingText` (`string`), `BudgetAccount` (`string?`), `Debit` (`decimal?`), `Credit` (`decimal?`), `ContactAccount` (`string?`) — matching `PostingLineDisplayerDto` field naming exactly, with no `SortOrder` property.
3. `PostingLineFeatureBase<TPostingJournalLineRequest>` exists in the same folder as an `internal abstract` class constrained to `TPostingJournalLineRequest : PostingJournalLineIdentificationRequestBase`, inherits `AccountingIdentificationFeatureBase<TPostingJournalLineRequest>`, exposes a protected get-only `StaticTextProvider` (`IStaticTextProvider`) property set via constructor, and provides a `sealed override Task ExecuteAsync(...)` that: fetches the posting journal via `AccountingGateway.GetPostingJournalAsync`, passes that exact instance to an abstract `Task ProcessPostingJournalAsync(ApplyPostingJournalModel postingJournal, TPostingJournalLineRequest request, CancellationToken cancellationToken)` — which mutates `postingJournal` in place rather than returning a replacement — then persists that same (now-mutated) instance via `AccountingGateway.SavePostingJournalAsync`.
4. `ValidationExceptionBase` exists in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces/Exceptions/`, is `public abstract`, inherits `Exception`, with a protected constructor taking `message`.
5. `IdentifierExceptionBase` exists in the same folder, is `public abstract`, inherits `ValidationExceptionBase`, adds a get-only `Identifier` (`Guid`) property, with a protected constructor taking `message` and `identifier`.
6. `IdentifierAlreadyExistsException` and `UnknownIdentifierException` exist in the same folder as `public` classes, both inherit `IdentifierExceptionBase`, each with a public constructor taking `identifier` (`Guid`) and an optional `message` (defaulting to `"The identifier already exists."` / `"The identifier is unknown."` respectively).
7. `ProblemDetailsFactory` maps `VerificationFailedException` (not `System.Security.VerificationException`) to a 400 Bad Request `ProblemDetails` using the exception's message.
8. `ProblemDetailsFactory` maps `IdentifierAlreadyExistsException` and `UnknownIdentifierException` to 400 Bad Request `ProblemDetails` using each exception's message.
9. `StaticTextKey` has new `IdentifierAlreadyExists` and `UnknownIdentifier` entries, registered in `StaticTextProvider` with the Danish texts `"Den angivne identifier eksisterer allerede."` and `"Den angivne identifier er ukendt."` respectively (both with 0 format arguments), and each covered by a `[TestCase]` in `GetStaticTextAsyncTests`.
10. `ValidationExceptionBase`, `IdentifierExceptionBase`, `IdentifierAlreadyExistsException`, and `UnknownIdentifierException` each have unit test coverage in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests` verifying: the exception's `Message` (including the default messages from criterion 6), the `Identifier` property is set from the constructor argument, and the inheritance chain (`is Exception`/`is ValidationExceptionBase`/`is IdentifierExceptionBase` as applicable).
11. `PostingJournalLineIdentificationRequestBase`, `PostingJournalLineDataRequestBase`, and `PostingLineFeatureBase<T>` each have unit test coverage in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests` verifying constructor argument assignment and, for `PostingLineFeatureBase<T>`, the `ExecuteAsync` orchestration: `GetPostingJournalAsync` is called with `request.AccountingNumber` and the given cancellation token; `ProcessPostingJournalAsync` is called with the exact `ApplyPostingJournalModel` instance returned by `GetPostingJournalAsync`, the given request, and the given cancellation token; `SavePostingJournalAsync` is called with `request.AccountingNumber`, that same instance (post-mutation), and the given cancellation token.
12. The corrected/new `ProblemDetailsFactory` mappings (`VerificationFailedException`, `IdentifierAlreadyExistsException`, `UnknownIdentifierException`) have unit test coverage in `OSDevGrp.OSIntranet.Bff.WebApi.Tests`, following existing test conventions (`[TestFixture]`, `[Category("UnitTest")]`, Moq/AutoFixture).
13. The solution builds cleanly and all unit tests pass.

## Scope

### In scope
- `PostingJournalLineIdentificationRequestBase`, `PostingJournalLineDataRequestBase`, `PostingLineFeatureBase<T>` in `OSDevGrp.OSIntranet.Bff.DomainServices`.
- `ValidationExceptionBase`, `IdentifierExceptionBase`, `IdentifierAlreadyExistsException`, `UnknownIdentifierException` in `OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces`.
- `ProblemDetailsFactory` fix and new exception mappings in `OSDevGrp.OSIntranet.Bff.WebApi`.
- New `StaticTextKey` entries and Danish translations.
- Unit tests for all of the above.

### Out of scope
- Concrete Add/Update/Delete posting-line command features (future PRDs).
- Any WebApi controller endpoint, route, or request/response DTO for adding/updating/removing posting lines.
- React UI changes.
- `SortOrder` handling — deferred to whichever future feature (Add/Update/Delete) needs it.

## Risks

- **Field type mismatch with the wire model:** `ApplyPostingLineModel` (NSwag-generated) uses `double?` for `Debit`/`Credit` and different property names (`AccountNumber`, `Details`). This foundation intentionally uses `decimal?`/`Account`/`PostingText` to match `PostingLineDisplayerDto`; the mapping between the two will need to happen inside whichever future feature implements `ProcessPostingJournalAsync`. Not a blocker for this PRD, but worth flagging so the mapping isn't overlooked later.
- **Exception applicability is asymmetric:** `IdentifierAlreadyExistsException` is only meaningful for "add" (a new identifier collides with an existing line), while `UnknownIdentifierException` is only meaningful for "update"/"delete" (referencing a line that doesn't exist). Both are prepared now even though only "add" is anticipated next, per explicit direction.
