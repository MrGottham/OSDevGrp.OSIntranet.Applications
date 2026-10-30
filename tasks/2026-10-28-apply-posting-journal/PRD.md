# PRD: Apply Posting Journal with Warning Notifications

## Problem

The React accounting page supports maintaining a draft posting journal but does not let an accounting modifier apply it. The underlying Web API already supports posting lines and returning accounting warnings, but that operation is not exposed through the BFF gateway, domain features, or React UI.

Users need to apply their draft, see refreshed accounting figures and the remaining draft, and read any returned warnings without those warnings disappearing automatically or being lost during the refresh.

This document covers the whole feature described in [TODO.md](../../TODO.md), including reusable request-scoped notification infrastructure. It incorporates the explicitly agreed clarifications below.

## Relevant Codebase

### What's there

- [IAccountingGateway](../../OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs:17) defines draft-journal retrieval and persistence. [AccountingGateway](../../OSDevGrp.OSIntranet.Bff.ServiceGateways/AccountingGateway.cs:100) delegates these operations to the generated client and translates both generic and nongeneric client exceptions.
- The generated [PostinglinesAsync contract](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:195) already accepts an accounting number, API version, posting lines, and cancellation token. [ApplyPostingJournalResultModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:4050) returns `PostingLines` and `PostingWarnings`. No generated-client signature change is required.
- [ApplyPostingLineModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:4071) represents posting text as `Details`. [PostingWarningModel](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:5007) exposes `Reason`, `Account`, `Amount`, and `PostingLine`; it has no `Warning` property. The supported [warning reasons](../../OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:5038) are account overdraft, income below budget, and expenses beyond budget.
- The existing backend [apply operation](../../OSDevGrp.OSIntranet.WebApi/Controllers/AccountingController.cs:415) returns posted lines and warnings. Draft-journal persistence is a separate operation.
- The command [AccountingIdentificationFeatureBase](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/AccountingIdentificationFeatureBase.cs:40) checks authentication, accounting access, and modifier permission for the requested accounting.
- [PostingLineFeatureBase](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/PostingLineFeatureBase.cs:37) retrieves a draft, processes it, and saves it. Its cloning helper sorts by posting date descending, then sort order descending, treating a missing sort order as zero. [DeletePostingLineFromPostingJournalFeature](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Commands/Accounting/DeletePostingLineFromPostingJournal/DeletePostingLineFromPostingJournalFeature.cs:26) demonstrates retaining lines by identifier before cloning.
- [PageFeatureBase](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/PageFeatureBase.cs:25) resolves page static texts using the request's format provider and cancellation token. [PageResponseBase](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/PageResponseBase.cs:11) carries those texts.
- [AccountingFeature](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/Accounting/AccountingFeature.cs:32) retrieves accounting data, posted lines, and the draft journal. [PostingJournalFeature](../../OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/PostingJournal/PostingJournalFeature.cs:45) supplies the journal-specific static texts. Both supply texts consumed by journal controls.
- [StaticTextProvider](../../OSDevGrp.OSIntranet.Bff.DomainServices/Logic/StaticText/StaticTextProvider.cs:17) formats Danish text using the supplied format provider. Its existing warning header is `Advarsel`. [AddDomainServices](../../OSDevGrp.OSIntranet.Bff.DomainServices/ServiceCollectionExtensions.cs:28) registers shared providers, transient helpers, and command/query features with the existing pipeline.
- [AccountingController](../../OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/AccountingController.cs:246) demonstrates modifier authorization, antiforgery protection, route validation, security-context resolution, command execution followed by a query, and response mapping.
- [AccountingIdentificationDto](../../OSDevGrp.OSIntranet.Bff.WebApi/Shared/Dtos/AccountingIdentificationDto.cs:7) exposes the accounting identifier as `Number`. [PostingJournalResponseDto](../../OSDevGrp.OSIntranet.Bff.WebApi/Controllers/Accounting/Dtos/PostingJournalResponseDto.cs:7) demonstrates response DTO mapping.
- React [Accounting](../../osdevgrp.osintranet.react/src/components/Accounting.jsx:25) owns accounting-page data and loads it through the accounting service. It renders [PostingJournal](../../osdevgrp.osintranet.react/src/components/Accounting.jsx:292) with a key based on the accounting number. [PostingJournal](../../osdevgrp.osintranet.react/src/components/PostingJournal.jsx:21) also owns local journal state, so refreshing the parent must actually refresh the displayed draft.
- [AccountingService](../../osdevgrp.osintranet.react/src/services/AccountingService.jsx:252) demonstrates required-argument checking, antiforgery headers, credentialed requests, JSON success responses, and shared error generation. [PostingJournal.handleDelete](../../osdevgrp.osintranet.react/src/components/PostingJournal.jsx:576) demonstrates journal-operation error presentation.
- Journal controls follow the [journal table](../../osdevgrp.osintranet.react/src/components/PostingJournal.jsx:411). The component already uses Bootstrap toasts, but its existing transient error/warning toasts are different from the new manually dismissed notifications. [package.json](../../osdevgrp.osintranet.react/package.json) contains React Bootstrap but no Markdown renderer or automated frontend test runner.

### How it works and where this fits

The new flow connects the React journal action to a BFF controller command. The command uses the accounting gateway to retrieve and apply the server-held draft, then saves a sorted journal containing only lines not identified as applied in the result. After that save succeeds, each returned warning is converted and published.

The controller then executes a notifications query in the same HTTP request and maps the drained notifications into its response. This is not a separate notification HTTP endpoint or a cross-request notification store.

React retains the returned notifications while reloading the current accounting. Once the reload succeeds, the accounting figures, posted lines, and draft reflect the reloaded response, and the retained notifications are displayed.

### Patterns and integration points

- Preserve the BFF's separation between contracts, implementations, test data, and tests, its CQS pipeline, inherited permissions, cancellation propagation, static-text conventions, and client-exception translation.
- Keep command and query notification dependencies connected to the same scoped collection even when feature instances are resolved independently.
- Follow the existing two-method fixture-extension pattern when creating notification mocks and existing model-helper patterns for gateway test data.
- Follow [GetAccountAsyncTests](../../OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests/AccountingGateway/GetAccountAsyncTests.cs:14) for gateway unit/integration conventions and configuration-based account selection, and [DeletePostingLineFromPostingJournalAsyncTests](../../OSDevGrp.OSIntranet.Bff.WebApi.Tests/Controllers/Accounting/AccountingController/DeletePostingLineFromPostingJournalAsyncTests.cs:21) for endpoint tests.
- Preserve existing journal editing, sorting, validation, and error presentation. Any shared cloning change must keep existing add, modify, and delete behavior intact.

## Goal

An authorized accounting modifier can apply a nonempty posting journal from the React accounting page, see the accounting and remaining draft refresh, and read every returned warning as a manually dismissible Markdown notification.

The BFF also provides reusable notification publishing and draining within a single request, without sharing notifications between requests.

## User Stories

- As an accounting modifier, I want to apply my draft journal from the accounting page so that I can complete bookkeeping without leaving the React application.
- As an accounting modifier, I want applied lines removed from the draft while unapplied lines remain so that the draft accurately represents work still outstanding.
- As an accounting user, I want the whole accounting refreshed after posting so that balances, posted lines, and the draft reflect the operation.
- As an accounting user, I want clear, locally formatted warning notifications that remain until I dismiss them so that I can review the consequences of posting.
- As a maintainer of BFF command features, I want request-isolated notification publishing and retrieval so that controllers can expose command notifications without cross-request leakage.

## Acceptance Criteria

### Notification infrastructure

1. `Logic/Notification` in DomainServices.Interfaces contains:
   - `INotification` with `NotificationFormat`, `Header`, and `Content` getters.
   - A `NotificationFormat` enum supporting `Markdown`.
   - `INotificationPublisher.PublishAsync(INotification notification, CancellationToken cancellationToken = default)`.
   - `INotificationRetriever.RetrieveAsync(CancellationToken cancellationToken = default)` returning `Task<IReadOnlyCollection<INotification>>`.
2. The internal `MarkdownNotification` accepts a header and Markdown content, exposes both supplied values, and reports the Markdown format.
3. Internal publisher and retriever implementations share a thread-safe request-scoped collection and are registered as scoped services by `AddDomainServices`. Separate requests/scopes never observe each other's notifications.
4. Retrieval drains notifications in publication order. A subsequent retrieval does not return items already retrieved. Concurrent publishing and retrieval do not duplicate or lose notifications; items not included in one drain remain available for a later drain. Simultaneous publications have the collection's publication order, not a prescribed ordering between concurrent callers.
5. Public `NotificationsRequest` and `NotificationsResponse` inherit from `PageRequestBase` and `PageResponseBase`. The response exposes `IReadOnlyCollection<INotification> Notifications`. The internal `NotificationsFeature` inherits from `PageFeatureBase<NotificationsRequest, NotificationsResponse, object>`, forwards cancellation to retrieval, returns the drained notifications, and includes `StaticTextKey.Notifications` in its static-text specifications.
6. `StaticTextKey.Notifications` resolves to `Notifikationer`.

### Applying and retaining journal lines

7. `IAccountingGateway` exposes `Task<ApplyPostingJournalResultModel> ApplyPostingJournalAsync(int accountingNumber, ApplyPostingJournalModel postingJournal, CancellationToken cancellationToken = default)`. Its implementation forwards the accounting number, the journal's posting lines, the existing API version, and cancellation token to `PostinglinesAsync`, returns the client's result, and preserves existing client-exception translation behavior.
8. The public `ApplyPostingJournalRequest` inherits from `AccountingIdentificationRequestBase` and exposes a constructor-initialized `IFormatProvider FormatProvider`. The internal command feature inherits from `AccountingIdentificationFeatureBase<ApplyPostingJournalRequest>` without overriding `VerifyPermissionAsync`.
9. Command execution retrieves the requested accounting's server-held journal, applies that journal for the same accounting, and saves a cloned journal excluding exactly the identifiers returned in the apply result's posted lines. Unapplied lines remain; the retrieved journal is not mutated. The retained lines preserve the existing descending posting-date/sort-order convention.
10. The command forwards its cancellation token to journal retrieval, applying, saving, warning conversion, and notification publication.
11. Only after saving the remaining journal succeeds does the command convert and publish each returned warning. The publisher receives each exact notification produced by the converter, in result warning order. An empty warning collection produces no notifications.

### Warning conversion and static texts

12. DomainServices.Interfaces exposes `IPostingWarningModelToNotificationConverter.ConvertAsync(PostingWarningModel postingWarningModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default)` returning `Task<INotification>`. Its internal implementation is registered using the existing helper-lifetime conventions.
13. All three supported warning reasons produce Markdown notifications. Each header comes from `StaticTextKey.Warning`, currently `Advarsel`; it does not depend on a nonexistent warning-model property. Unsupported reasons throw `NotSupportedException` identifying the unsupported reason.
14. Warning content uses the matching static-text key and the supplied format provider. Its six arguments, in order, are `Account.AccountName`, `Amount` formatted as currency, `Environment.NewLine`, `Environment.NewLine`, `PostingLine.PostingDate` formatted as a short date with the supplied format provider, and `PostingLine.Details`. Both header and content lookups receive the supplied format provider and cancellation token.
15. The added static-text keys resolve to these exact Danish values:

   | Key | Text | Argument count |
   | --- | --- | --- |
   | `ApplyPostingJournal` | `Bogfør kladde` | 0 |
   | `AccountIsOverdrawn` | `Kontoen med navnet **{0}** er i overtræk med **{1}**{2}{3}{4}: {5}` | 6 |
   | `ExpectedIncomeHasNotBeenReachedYet` | `Budgetkontoen med navnet **{0}** har endnu ikke nået sit budgetterede mål på **{1}**{2}{3}{4}: {5}` | 6 |
   | `ExpectedExpensesHaveAlreadyBeenReached` | `Budgetkontoen med navnet **{0}** har overskredet sit budgetterede mål på **{1}**.{2}{3}{4}: {5}` | 6 |

16. Both `AccountingFeature` and `PostingJournalFeature` include `StaticTextKey.ApplyPostingJournal` in their static-text specifications.

### BFF HTTP contract

17. Shared `NotificationDto` exposes init-only string properties `Format`, `Header`, and `Content`. Each has `[Required]` and `[MinLength(...)]`, with a separate minimum-length constant of `1` in `ValidationValues`. `Format` also has a regular-expression constraint permitting only the exact value `Markdown`. Its internal static mapper maps an `INotification` to these values.
18. `ApplyPostingJournalResponseDto` inherits from `AccountingIdentificationDto`, exposes an init-only `IReadOnlyCollection<NotificationDto> Notifications`, and maps the requested accounting number and `NotificationsResponse`. The JSON uses the inherited `number` field and a `notifications` array whose items contain `format`, `header`, and `content`.
19. `AccountingController.ApplyPostingJournalAsync` is exposed as `POST {accountingNumber:int}/postingjournal/apply`, with accounting-modifier authorization, antiforgery validation, and existing accounting-number route validation. It declares `ApplyPostingJournalResponseDto` for HTTP 200 and `ProblemDetails` for HTTP 400, 401, and 500.
20. The endpoint resolves the current security context and executes the apply command for the requested accounting using the controller's format provider and cancellation token. Only after command success does it execute the notifications query within the same request and return the mapped response. No request-body journal or human-verification challenge is added.
21. Command or notifications-query failures surface through existing WebApi error behavior rather than becoming HTTP 200 responses. Gateway failures propagate and do not produce success notifications.

### React behavior

22. `StaticTextHelper.getApplyPostingJournalText` resolves `ApplyPostingJournal`. `AccountingService.applyPostingJournal(accountingNumber)` validates the required accounting number using the existing delete-service pattern, sends a credentialed POST with the generated antiforgery header, returns successful JSON, and uses existing error generation for failures.
23. `PostingJournal` displays a right-aligned button group after its table only when at least one draft line exists. Its apply button uses the static-text helper. Apply, add, modify, and delete buttons are disabled while applying, preventing another mutation from being started through those controls during submission.
24. Applying uses the accounting service and the same error presentation as `handleDelete`. On successful application, the component passes the response notifications to its `onPostingJournalApplied` callback.
25. `Accounting` retains those notifications and reloads the full current accounting. After successful refresh, accounting figures, posted lines, and the displayed draft reflect the response, including removal of applied draft lines despite the child's local state.
26. `Notifications` accepts items containing `format`, `header`, and `content`. Each notification appears as a toast on the right side of the screen with its header and Markdown-rendered content when the format is `Markdown`. Each toast requires individual manual dismissal; there is no automatic timeout.
27. New warnings append to undismissed notifications for the current accounting. Notifications survive the successful accounting refresh, but are cleared when leaving the accounting page or switching accounting and are not persisted across browser reloads. No-warning success still refreshes the accounting and does not create a synthetic notification.
28. If the accounting refresh fails after applying, the existing accounting error boundary handles the error. Returned notifications are not displayed unless the refresh succeeds.

### Verification

29. ServiceGateways tests and TestData include apply-result, posted-line, and warning fixture helpers following existing patterns. Unit tests cover forwarding of the accounting number, posting lines, API version, and cancellation token, returning the result, and consistent translation of client errors.
30. The gateway integration scenario uses configuration from `AccountingTestOptions`, following `GetAccountAsyncTests`, for the accounting, account, budget-account, and contact-account numbers. It posts two matching lines with `Details = "Test"`, debit `1000` on the first and credit `1000` on the second. After a successful verification run, that integration test is marked ignored for subsequent runs; its unit tests remain active.
31. DomainServices tests cover Markdown construction, publication and draining order, concurrent access, isolation between scopes, all warning reasons and unsupported reasons, exact formatting inputs and cancellation forwarding, notifications-query results/static texts, apply-command sequencing and arguments, exact published converter outputs, retained-line correctness, and gateway failure propagation without success notifications. Related existing journal-command behavior remains covered.
32. The existing parameterized static-text tests include `Notifications`, `ApplyPostingJournal`, and the three warning texts with their expected values and argument counts. Accounting and posting-journal query tests verify inclusion of the apply label.
33. DomainServices and WebApi test fixtures provide notification mock helpers following their existing two-method fixture-extension pattern. WebApi tests cover command invocation, notification querying only after success, response accounting number and notification mapping, cancellation, and surfaced command/query failures.
34. React acceptance is verified manually: nonempty/empty journal controls, disabled mutations during submission, successful application with and without warnings, refreshed accounting and draft, Markdown display, manual dismissal, appending warnings across applies, clearing on navigation/account changes, and existing failure presentation. No automated React tests are required.

## Scope

### In scope

- The complete notification and posting-journal apply functionality described above across BFF ServiceGateways.Interfaces, ServiceGateways, DomainServices.Interfaces, DomainServices, WebApi, and React.
- Corresponding backend coverage in ServiceGateways.Tests, ServiceGateways.TestData, DomainServices.Tests, and WebApi.Tests.
- Request-scoped notification contracts, publishing, draining, query feature, mapping, Danish static texts, and the three existing warning reasons.
- Existing permission enforcement, antiforgery protection, validation, cancellation, and error conventions.
- Full-accounting refresh and correct React journal-state refresh, with page-local, manually dismissed notifications.
- The specified integration verification and subsequent ignored-test state, plus manual frontend acceptance.

### Out of scope

- Changes to accounting rules, warning calculation, database schema, or the underlying Web API/generated-client contract.
- Changes to the MVC application.
- A separate notifications HTTP endpoint, cross-request notification storage, push delivery, or browser-persistent notifications.
- Notification formats or posting-warning reasons beyond those specified here.
- A new confirmation dialog or human-verification challenge for applying.
- Transactional rollback across posting and draft persistence, automatic retries, duplicate-post prevention, or recovery workflows after partial success.
- Automated React tests, frontend test-runner setup, and unrelated refactoring.

## Risks

- **Partial success is explicitly accepted.** Posting, draft persistence, warning conversion, and the notifications query are separate operations. Posting may already have succeeded when a later step fails. Errors must surface, but rollback, automatic retry, and duplicate-post prevention are not included. Retrying after a failed draft save can risk reposting lines.
- **Cancellation can occur after a side effect.** Propagating cancellation does not make the multi-operation flow transactional or undo lines already posted.
- **Concurrent journal changes are not protected by a new concurrency protocol.** UI disabling prevents local mutations during submission but does not lock other tabs or clients.
- **React has two journal-state owners.** Reloading accounting data alone is insufficient if `PostingJournal` retains its original local state. Acceptance requires observing the refreshed draft, not merely a successful fetch.
- **Notification scope and draining must agree.** Independent publisher/retriever resolutions must share the same request collection; concurrent drains must not duplicate or lose items. Notification state cannot be recovered by a later HTTP request.
- **Refresh failure hides returned warnings under the agreed behavior.** The existing accounting error boundary takes precedence until a refresh succeeds; no persistent fallback notification store is included.
- **The integration test writes accounting data.** Its verification requires the configured external services and accounting test data. The successful run and later ignored status must be distinguished from unit-only validation; an ignored test alone is not evidence that integration succeeded.
- **Warning rendering crosses a data/presentation boundary.** Supplied account names become part of Markdown content, and the frontend currently has no Markdown renderer. The chosen rendering must support the specified presentation without treating notification content as executable markup.
