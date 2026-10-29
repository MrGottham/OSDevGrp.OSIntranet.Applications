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
- [ ] Add `INotificationPublisher` in `Logic/Notification` with `Task PublishAsync(INotification notification)`.
- [ ] Add `INotificationRetriever` in `Logic/Notification` with `Task<IReadOnlyCollection<INotification>> RetrieveAsync()`.
- [ ] Add `Notifications` to `StaticTextKey`.

#### BFF DomainServices

- [ ] Add the internal `MarkdownNotification` class under `Logic/Notification`, implementing `INotification`; accept the header and Markdown content in its constructor, return `NotificationFormat.Markdown`, and expose the supplied header and content.
- [ ] Add internal `NotificationPublisher` and `NotificationRetriever` classes under `Logic/Notification`, implementing `INotificationPublisher` and `INotificationRetriever` respectively, sharing a thread-safe request-scoped notification collection.
- [ ] Register `NotificationPublisher` and `NotificationRetriever` as scoped services in `AddDomainServices` so notifications are isolated between requests.
- [ ] Add the Danish text `Notifikationer` for `StaticTextKey.Notifications` in `StaticTextProvider`.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsRequest.cs` as a public `NotificationsRequest` inheriting from `PageRequestBase`.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsResponse.cs` as a public `NotificationsResponse` inheriting from `PageResponseBase` with an `IReadOnlyCollection<INotification> Notifications` getter.
- [ ] Add `Features/Queries/Notification/Notifications/NotificationsFeature.cs` as an internal query feature inheriting from `PageFeatureBase<NotificationsRequest, NotificationsResponse, object>`; have `ExecuteAsync` use `INotificationRetriever` to retrieve published notifications and return them in `NotificationsResponse`, and specify `StaticTextKey.Notifications` in `GetStaticTextSpecifications`.

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

- [ ] Add an accounting gateway operation for applying a posting journal and returning the backend apply result, including any warnings.

#### BFF ServiceGateways

- [ ] Implement the apply operation using the generated WebApi client and follow the existing service-gateway exception translation pattern.

#### BFF ServiceGateways.Tests and TestData

- [ ] Add test data for the posting-journal apply result and its warnings.
- [ ] Verify the gateway calls the generated client with the correct accounting and posting-journal data, returns the apply result, and translates client errors consistently.

#### BFF DomainServices.Interfaces

- [ ] Define the request data needed by the posting-journal apply command, following the existing accounting command request patterns.

#### BFF DomainServices

- [ ] Add an apply-posting-journal command feature that validates and authorizes the accounting operation, then calls the accounting gateway.
- [ ] Convert warnings from the apply result into Markdown notifications using the notification publisher.
- [ ] After a successful apply, allow the WebApi to query and return the refreshed accounting.

#### BFF DomainServices.Tests

- [ ] Verify the command invokes the gateway with the requested accounting and posting journal.
- [ ] Verify returned warnings are published as notifications with the expected Markdown format, header, and content.
- [ ] Verify gateway failures are propagated and do not produce success notifications.

#### BFF WebApi

- [ ] Add an authorized endpoint to apply the posting journal for an accounting, using the established route, validation, and response conventions.
- [ ] Execute the apply command, retrieve the refreshed accounting after success, and expose both the refreshed response and published notifications to the React application.

#### BFF WebApi.Tests

- [ ] Verify the endpoint executes the apply command for the requested accounting, queries the refreshed accounting after success, and returns the result with notifications.
- [ ] Verify command or refresh-query failures are surfaced using the existing WebApi error behavior.

#### React Application

- [ ] Add an AccountingService operation for applying the posting journal and handle unsuccessful responses using existing service conventions.
- [ ] Add the user action to apply the posting journal for the selected accounting.
- [ ] On success, reload the accounting and present any returned warning notifications to the user.
- [ ] Manually verify the apply flow, accounting refresh, and warning notification presentation.
