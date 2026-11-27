# Implementing contacts into the React application

## General informations

We gona implement logic in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces
* OSDevGrp.OSIntranet.Bff.ServiceGateways
* OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces
* OSDevGrp.OSIntranet.Bff.DomainServices

We gona create tests for functionality in the following projects:

* OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData
* OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests
* OSDevGrp.OSIntranet.Bff.DomainServices.Tests

## Simplify the way the accounting domain retrieve letter heads

### Business goal

Our backend WebApi now supports getting letter heads when the calling user has the accounting claim. Therefore we do not need to check where the calling user has the common data acccess within our BFF features as it happens in:

* AccountingFeature
* AccountingPreCreationFeature

Those features should use a new service gateway method on IAccountingGateway to retreive letter heads by calling the method which gets letter heads from the accounting area in the WebApiClient. The logic around getting letter heads should also be simplified in the features as we can expect the user already has the accounting claim.

### Acceptance criterias

* IAccountingGateway exposes a new method named GetLetterHeadsAsync which use the WebApiClient to get letter heads from the accounting area
* GetLetterHeadsAsync at IAccountingGateway are tested with both unit tests and a integrations test
* The AccountingFeature and AccountingPreCreationFeature should use GetLetterHeadsAsync at IAccountingGateway to get the letter heads
* Logic around getting letters head in AccountingFeature and AccountingPreCreationFeature are simplified
  * ICommonGateway are removed as dependency
  * Calls to HasCommonDataAccess on IPermissionChecker are removed
* Test methods only test one thing
