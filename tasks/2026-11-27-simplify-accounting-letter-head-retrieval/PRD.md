# PRD: Simplify Accounting Letter-Head Retrieval

## Problem

The Accounting and Accounting Pre-Creation features retrieve letter heads through the Common area and condition their behavior or access on Common-data access. The WebApi already provides letter heads through the Accounting area for callers with the accounting claim, so these features should use that capability instead.

## Relevant Codebase

- **What's there**:
  - `OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces/IAccountingGateway.cs:5` defines the accounting gateway contract. `OSDevGrp.OSIntranet.Bff.ServiceGateways/AccountingGateway.cs:7` implements it through `IWebApiClient`.
  - `OSDevGrp.OSIntranet.Bff.ServiceGateways/CommonGateway.cs:20` currently retrieves letter heads through the Common endpoint. The generated client exposes the Accounting endpoint at `OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs:3099` and the Common endpoint at line 3215.
  - `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/Accounting/AccountingFeature.cs:36` loads the accounting data; its `ResolveLetterHeadsAsync` checks Common-data access and either uses the accounting's existing letter head or calls `ICommonGateway` (line 103).
  - `OSDevGrp.OSIntranet.Bff.DomainServices/Features/Queries/Accounting/AccountingPreCreation/AccountingPreCreationFeature.cs:44` retrieves Common letter heads for its response. Its permission check also requires Common-data access (line 76).
  - Gateway tests are in `OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests`; the existing Common letter-head tests show unit and integration coverage in `CommonGateway/GetLetterHeadsAsyncTests.cs`. The target feature tests are in the corresponding Accounting and AccountingPreCreation folders in `OSDevGrp.OSIntranet.Bff.DomainServices.Tests`.
- **How it works**: `AccountingFeature` gets accounting, posting-line, and journal data, then resolves letter heads based on the user's Common-data access. `AccountingPreCreationFeature` fetches Common letter heads while concurrently building static text and validation rules. Both features map letter-head models to `LetterHeadIdentificationModel` for their responses.
- **Patterns to follow**: `AccountingGateway` methods call the matching generated WebApi client operation with API version 1.0 and pass through the cancellation token. They translate both generic and non-generic `WebApiClientException` instances into service-gateway exceptions. Gateway tests cover client invocation, arguments, results, errors, and an integration call.
- **Integration points**: `IAccountingGateway` is registered by the service-gateway assembly's automatic interface registration. The feature responses continue to expose letter heads as identification models; the generated WebApi client is already the integration point for the Accounting endpoint.

## Goal

Both accounting features obtain letter heads from the Accounting area through `IAccountingGateway`, without depending on Common-data access or `ICommonGateway`.

## User Stories

- As an accounting user, I want Accounting letter heads available in the Accounting feature without Common-data access so that the feature uses the Accounting capability provided by the WebApi.
- As an authenticated accounting creator, I want to use Accounting Pre-Creation without Common-data access so that creation is not blocked by an unrelated Common-data permission.

## Acceptance Criteria

1. `IAccountingGateway` exposes `GetLetterHeadsAsync` and `AccountingGateway` implements it by calling the generated WebApi client's Accounting letter-head operation, forwarding the API version and cancellation token and translating WebApi client exceptions according to existing gateway behavior.
2. `GetLetterHeadsAsync` has unit tests and an integration test.
3. `AccountingFeature` and `AccountingPreCreationFeature` obtain and map letter heads through `IAccountingGateway.GetLetterHeadsAsync`.
4. Neither target feature depends on `ICommonGateway` or calls `HasCommonDataAccess` on `IPermissionChecker`.
5. `AccountingFeature` no longer switches letter-head retrieval based on Common-data access; it uses the Accounting letter-head results.
6. `AccountingPreCreationFeature` permission verification continues to require authentication, accounting access, and accounting-creator status, but not Common-data access.
7. Tests added or changed for this work each verify one behavior.

## Scope

### In scope

- The Accounting gateway contract and implementation in `OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces` and `OSDevGrp.OSIntranet.Bff.ServiceGateways`.
- Unit and integration tests in `OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests`.
- Both named domain-service features and their tests in `OSDevGrp.OSIntranet.Bff.DomainServices` and `OSDevGrp.OSIntranet.Bff.DomainServices.Tests`.

### Out of scope

- Contact functionality mentioned in the TODO's general heading; the detailed task and acceptance criteria concern letter-head retrieval only.
- Removing or changing the Common gateway's letter-head operation for other callers.
- Changes to the generated WebApi client or WebApi endpoint.

## Risks

- Removing Common-data access from Accounting Pre-Creation permission verification changes who may use that feature. Authentication, accounting access, and accounting-creator checks remain in place; the Accounting endpoint must support the intended accounting-claim authorization.
- The integration test follows the existing gateway test setup, which uses test configuration from user secrets and requires the WebApi integration environment to be available.
