# Appending posting journal for an accounting

## General

We need to append/implement functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices
* OSDevGrp.OSIntranet.Bff.WebApi
* osdevgrp.osintranet.react

We need to create tests for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests
* OSDevGrp.OSIntranet.Bff.WebApi.Tests

Note: No automated tests are needed for osdevgrp.osintranet.react as the React component will be validated through manual testing.

## Adding notifications in the BFF domain service layer

### Business Goal

Command features should be able to publish notifications which a controller method in the WebApi could retrieve and expose to the react application. The notification should include a notification format (markdown), header and content.

### Implementation Bullets

#### BFF DomainServices.Interfaces

- [ ] Add `INotification` under `Logic/Notification` with `NotificationFormat`, `Header`, and `Content` getters, and define the `NotificationFormat` enum in the same folder with `Markdown` as a supported value.
- [ ] Add `INotificationPublisher` in `Logic/Notification` with `Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)`.
- [ ] Add `INotificationRetriever` in `Logic/Notification` with `Task<IReadOnlyCollection<INotification>> RetrieveAsync(CancellationToken cancellationToken = default)`.
- [ ] Add `Notifications` to `StaticTextKey`.

#### BFF DomainServices

- [ ] Add the internal `MarkdownNotification` class under `Logic/Notification`, implementing `INotification`; accept the header and Markdown content in its constructor, return `NotificationFormat.Markdown`, and expose the supplied header and content.
- [ ] Add internal `NotificationPublisher` and `NotificationRetriever` classes under `Logic/Notification`, implementing `INotificationPublisher` and `INotificationRetriever` respectively, sharing a thread-safe request-scoped notification collection.
- [ ] Register `NotificationPublisher` and `NotificationRetriever` as scoped services in `AddDomainServices` so notifications are isolated between requests.
- [ ] Add the Danish text `Notifikationer` for `StaticTextKey.Notifications` in `StaticTextProvider`.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsRequest.cs` as a public `NotificationsRequest` inheriting from `PageRequestBase`.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsResponse.cs` as a public `NotificationsResponse` inheriting from `PageResponseBase` with an `IReadOnlyCollection<INotification> Notifications` getter.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsFeature.cs` as an internal query feature inheriting from `PageFeatureBase<NotificationsRequest, NotificationsResponse, object>`; have `ExecuteAsync` pass its cancellation token to `INotificationRetriever.RetrieveAsync`, return the published notifications in `NotificationsResponse`, and specify `StaticTextKey.Notifications` in `GetStaticTextSpecifications`.

#### BFF DomainServices.Tests

- [ ] In `OSDevGrp.OSIntranet.Bff.DomainServices.Tests`, add `CreateNotification` to `FixtureExtensions`, following the existing two-method fixture-extension pattern and mocking `INotification`.
- [ ] Add `Logic/Notification/MarkdownNotification/ConstructorTests.cs` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests` to verify the `MarkdownNotification` constructor sets the Markdown format, header, and content.
- [ ] Add `Logic/Notification/NotificationPublisher/PublishAsyncTests.cs` to test `NotificationPublisher.PublishAsync`, including publication order and safe concurrent publication.
- [ ] Add `Logic/Notification/NotificationRetriever/RetrieveAsyncTests.cs` to test `NotificationRetriever.RetrieveAsync`, including retrieval order, safe concurrent access, and isolation between request scopes.
- [ ] Add a test case for `StaticTextKey.Notifications` with expected text `Notifikationer` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsFeature/ExecuteAsyncTests.cs` in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests`, following the existing `ExecuteAsyncTests` pattern; verify `INotificationRetriever.RetrieveAsync` is called and the response contains the expected static text values, with each static-text expectation covered by a test case.

#### BFF WebApi

- [ ] Add `Shared/Dtos/NotificationDto.cs` with string `Format`, `Header`, and `Content` properties, each with `get` and `init`, and decorate each property with `[Required]` and `[MinLength(...)]`; define a separate minimum-length constant with value `1` for each property in `ValidationValues`.
- [ ] Add a `ValidationValues` regex pattern that permits only `Markdown`, and apply it to `NotificationDto.Format` with `[RegularExpression(...)]`.
- [ ] Add an internal static `NotificationDto.Map` method that accepts an `INotification` and maps its format, header, and content to the DTO.

#### BFF WebApi.Tests

- [ ] Add `CreateNotification` to `FixtureExtensions`, following the existing two-method fixture-extension pattern and mocking `INotification`.

## Apply posting journal for a given accounting

### Business Goal

In the react application an user should be able to apply the posting journal for a given accounting. When the posting journal has been successfully applied the react application should reload the accounting and if some warnings has been returned from the backend then they should be converter to notifications and be presentet for the user in the react application.

### Implementation Bullets

#### BFF ServiceGateways.Interfaces

- [ ] Add `Task<ApplyPostingJournalResultModel> ApplyPostingJournalAsync(int accountingNumber, ApplyPostingJournalModel postingJournal, CancellationToken cancellationToken = default)` to `IAccountingGateway`.

#### BFF ServiceGateways

- [ ] Implement `AccountingGateway.ApplyPostingJournalAsync` by calling the generated client's `PostinglinesAsync` with the supplied accounting number, the posting lines from `postingJournal`, and the cancellation token; return the apply result and follow the existing service-gateway exception translation pattern.

#### BFF ServiceGateways.Tests and TestData

- [ ] Extend `FixtureExtensions` with methods for creating test data for `ApplyPostingJournalResultModel`, including its `PostingLines` and `PostingWarnings`, following the existing helper patterns in the class.
- [ ] Add unit and integration tests for `ApplyPostingJournalAsync` in the `AccountingGateway` folder, in a test class named `ApplyPostingJournalAsyncTests`, following the patterns used to test the other gateway methods.
- [ ] Verify the tests cover forwarding the requested accounting number, posting lines, and cancellation token to `PostinglinesAsync`, returning its `ApplyPostingJournalResultModel`, and consistently translating client errors.
- [ ] In the integration test, apply a posting journal with two matching lines, each with `PostingText` set to `"Test"`: set `Debit` to `1000` on the first line and `Credit` to `1000` on the second. Resolve the accounting number, account number, budget account number, and contact account number from `AccountingTestOptions`, following the integration-test pattern in `GetAccountAsyncTests`.
- [ ] Once the integration scenario has successfully verified the logic, mark that integration test as ignored so it is skipped in subsequent test runs; keep the unit tests active.

#### BFF DomainServices.Interfaces

- [ ] Add the public `IPostingWarningModelToNotificationConverter` in `Logic/Notification` with `Task<INotification> ConvertAsync(PostingWarningModel postingWarningModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default)`.
- [ ] Add `ApplyPostingJournal` to `StaticTextKey`.
- [ ] Add `AccountIsOverdrawn`, `ExpectedIncomeHasNotBeenReachedYet`, and `ExpectedExpensesHaveAlreadyBeenReached` to `StaticTextKey` for posting-warning notifications.
- [ ] Include `StaticTextKey.ApplyPostingJournal` in `GetStaticTextSpecifications` in both `AccountingFeature` and `PostingJournalFeature`.

#### BFF DomainServices

- [ ] Add the internal `PostingWarningModelToNotificationConverter` in `Logic/Notification`, implementing `IPostingWarningModelToNotificationConverter`.
- [ ] Implement `ConvertAsync` to return a `MarkdownNotification` for `AccountIsOverdrawn`, `ExpectedIncomeHasNotBeenReachedYet`, and `ExpectedExpensesHaveAlreadyBeenReached`. Use `IStaticTextProvider` with the supplied `formatProvider` to resolve the header from `StaticTextKey.Warning` and the Danish Markdown content for the matching reason. Supply arguments in order: `0` is `Account.AccountName`, `1` is `Amount` formatted as currency with `formatProvider`, `2` and `3` are `Environment.NewLine`, `4` is `PostingLine.PostingDate` formatted as a short date with `formatProvider`, and `5` is `PostingLine.Details`; pass the cancellation token to the text lookups. Throw `NotSupportedException` with information about the unknown reason for any unsupported reason.
- [ ] Register `PostingWarningModelToNotificationConverter` for `IPostingWarningModelToNotificationConverter` in `AddDomainServices`, following the existing service-lifetime conventions.
- [ ] Configure `StaticTextProvider` to return `Bogfør kladde` for `StaticTextKey.ApplyPostingJournal`.
- [ ] Configure `StaticTextProvider` to return `Kontoen med navnet **{0}** er i overtræk med **{1}**{2}{3}{4}: {5}` for `StaticTextKey.AccountIsOverdrawn`.
- [ ] Configure `StaticTextProvider` to return `Budgetkontoen med navnet **{0}** har endnu ikke nået sit budgetterede mål på **{1}**{2}{3}{4}: {5}` for `StaticTextKey.ExpectedIncomeHasNotBeenReachedYet`.
- [ ] Configure `StaticTextProvider` to return `Budgetkontoen med navnet **{0}** har overskredet sit budgetterede mål på **{1}**.{2}{3}{4}: {5}` for `StaticTextKey.ExpectedExpensesHaveAlreadyBeenReached`.
- [ ] Add the public `ApplyPostingJournalRequest` in `Features/Commands/Accounting/ApplyPostingJournal`, inheriting from `AccountingIdentificationRequestBase` and exposing a constructor-initialized `IFormatProvider FormatProvider` getter.
- [ ] Add the internal `ApplyPostingJournalFeature` in `Features/Commands/Accounting/ApplyPostingJournal`, inheriting from `AccountingIdentificationFeatureBase` and using `ApplyPostingJournalRequest` as `TAccountingIdentificationRequest`; inject `IPostingWarningModelToNotificationConverter` and `INotificationPublisher` through its constructor.
- [ ] Do not override `VerifyPermissionAsync` in `ApplyPostingJournalFeature`; use its inherited implementation.
- [ ] In `ExecuteAsync`, retrieve the posting journal for the requested accounting number with `IAccountingGateway.GetPostingJournalAsync`, then call `IAccountingGateway.ApplyPostingJournalAsync` with that accounting number and the retrieved journal; pass the request cancellation token to both calls.
- [ ] Use the applied-line identifiers in the apply result to exclude those lines from the retrieved posting journal; refactor `SortAndClonePostingJournal` as needed so it can clone the journal while excluding specified lines.
- [ ] Save the cloned journal without the applied lines through `IAccountingGateway.SavePostingJournalAsync`, using the requested accounting number and cancellation token.
- [ ] After saving the posting journal, use the injected converter and publisher to convert each `PostingWarningModel` from the apply result into an `INotification` and publish it, passing the cancellation token to both async calls.

#### BFF DomainServices.Tests

- [ ] Add `Logic/Notification/PostingWarningModelToNotificationConverter/ConvertAsyncTests.cs` with a `ConvertAsyncTests` class. Cover all three supported reasons and the unknown-reason `NotSupportedException`; verify `IStaticTextProvider` is called with the correct keys, short-date/account-name/currency arguments, supplied `IFormatProvider`, and cancellation token.
- [ ] Add a test case for `StaticTextKey.ApplyPostingJournal` with expected text `Bogfør kladde` and argument count `0` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Add a `StaticTextKey.ApplyPostingJournal` test case to the `ExecuteAsyncTests` for both `AccountingFeature` and `PostingJournalFeature`, verifying the key is included in each feature's static-text specifications.
- [ ] Add a test case for `StaticTextKey.AccountIsOverdrawn` with expected text `Kontoen med navnet **{0}** er i overtræk med **{1}**{2}{3}{4}: {5}` and argument count `6` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Add a test case for `StaticTextKey.ExpectedIncomeHasNotBeenReachedYet` with expected text `Budgetkontoen med navnet **{0}** har endnu ikke nået sit budgetterede mål på **{1}**{2}{3}{4}: {5}` and argument count `6` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Add a test case for `StaticTextKey.ExpectedExpensesHaveAlreadyBeenReached` with expected text `Budgetkontoen med navnet **{0}** har overskredet sit budgetterede mål på **{1}**.{2}{3}{4}: {5}` and argument count `6` to `GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticText`.
- [ ] Add `Features/Commands/Accounting/ApplyPostingJournal/ApplyPostingJournalFeature/ExecuteAsyncTests.cs` with an `ExecuteAsyncTests` class, following the existing command-feature test pattern.
- [ ] Verify `ExecuteAsync` calls `GetPostingJournalAsync`, `ApplyPostingJournalAsync`, and `SavePostingJournalAsync` with the requested accounting number, the retrieved journal, the cloned journal with applied lines removed, and the cancellation token as appropriate.
- [ ] Mock `IPostingWarningModelToNotificationConverter` and verify `ConvertAsync` is called for each returned warning with the request's `FormatProvider` and cancellation token; verify `INotificationPublisher.PublishAsync` receives each exact notification returned by the converter, with the cancellation token.
- [ ] Verify gateway failures are propagated and do not produce success notifications.

#### BFF WebApi

- [ ] Add `ApplyPostingJournalResponseDto` inheriting from `AccountingIdentificationDto`, with an init-only `IReadOnlyCollection<NotificationDto> Notifications` property and an internal `Map` method accepting an accounting number and `NotificationsResponse`; map the accounting number and the response's notifications to the DTO.
- [ ] Add `ApplyPostingJournalAsync` to `AccountingController` with `[Authorize(Policy = Policies.AccountingModifier)]`, `[HttpPost("{accountingNumber:int}/postingjournal/apply")]`, `[ValidateAntiForgeryToken]`, the specified response metadata for `ApplyPostingJournalResponseDto` (200) and `ProblemDetails` (400, 401, 500), and injected `ICommandFeature<ApplyPostingJournalRequest>` and `IQueryFeature<NotificationsRequest, NotificationsResponse>` dependencies, a validated accounting-number route parameter, and a cancellation token.
- [ ] In the endpoint, execute the apply command and, on success, query `NotificationsResponse`; return `ApplyPostingJournalResponseDto` mapped from the accounting number and notifications.

#### BFF WebApi.Tests

- [ ] Add `Controllers/Accounting/AccountingController/ApplyPostingJournalAsyncTests.cs` with an `ApplyPostingJournalAsyncTests` class, following the endpoint test pattern in `DeletePostingLineFromPostingJournalAsyncTests`.
- [ ] Verify the endpoint executes the apply command for the requested accounting, queries notifications after success, and returns `ApplyPostingJournalResponseDto` with the accounting number and notifications.
- [ ] Verify command or notifications-query failures are surfaced using the existing WebApi error behavior.

#### React Application

- [ ] Add `getApplyPostingJournalText` to `StaticTextHelper`, resolving the static text for `ApplyPostingJournal`.
- [ ] Add the async `applyPostingJournal` function to `AccountingService`, accepting an accounting number. Validate the accounting number following `deletePostingLineFromPostingLineJournal`, then POST to `{accountingNumber}/postingjournal/apply` with `generateAntiforgeryHeader` and `credentials`; use the existing error handling and return the JSON response when successful.
- [ ] In `Accounting.jsx`, add an `onPostingJournalApplied` function that accepts and retains the notifications while forcing the current accounting to reload; pass it to `PostingJournal` as its `onPostingJournalApplied` prop.
- [ ] Add a React component named `Notifications` that accepts a `notifications` prop containing items with `format`, `header`, and `content`. Display each notification as a toast on the right side of the screen, show its header, and render its content as Markdown when `format` is `Markdown`; require users to close each toast manually.
- [ ] In `PostingJournal`, add a right-aligned button group after the `Table`; show it only when the posting journal contains at least one posting line, and include an apply button whose text is resolved by `getApplyPostingJournalText` from `StaticTextHelper`.
- [ ] Add an `applyPostingJournal` function in `PostingJournal.jsx`, called by the apply button. Have it disable the apply, add, modify, and delete buttons while the operation is in progress, call `AccountingService.applyPostingJournal`, and use the same error handling as `handleDelete`. On success, pass the notifications from the JSON response to the `onPostingJournalApplied` callback supplied to the `PostingJournal` component as a prop.
- [ ] After the accounting reload completes, display the retained notifications using the `Notifications` component.
- [ ] Manually verify the apply flow, accounting refresh, and warning notification presentation.
