using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.AccountingIdentificationFeatureBase;

public abstract class AccountingIdentificationFeatureTestBase
{
    #region Methods

    protected static IPermissionVerifiable<TRequest> CreateSut<TRequest>(
        Fixture fixture,
        Mock<IPermissionChecker> permissionCheckerMock,
        Mock<IAccountingGateway> accountingGatewayMock,
        bool isAuthenticated = true,
        bool hasAccountingAccess = true,
        bool isAccountingModifier = true)
        where TRequest : AccountingIdentificationRequestBase
    {
        permissionCheckerMock.Setup(fixture,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        return new TestAccountingIdentificationCommandFeature<TRequest>(
            permissionCheckerMock.Object,
            accountingGatewayMock.Object);
    }

    protected static ICommandFeature<TRequest> CreateSut<TRequest>(
        Fixture fixture,
        Mock<IPermissionChecker> permissionCheckerMock,
        Mock<IAccountingGateway> accountingGatewayMock,
        Func<TRequest, CancellationToken, Task>? executeAsyncDelegate = null,
        bool isAuthenticated = true,
        bool hasAccountingAccess = true,
        bool isAccountingModifier = true)
        where TRequest : AccountingIdentificationRequestBase
    {
        permissionCheckerMock.Setup(fixture,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        return new TestAccountingIdentificationCommandFeature<TRequest>(
            permissionCheckerMock.Object,
            accountingGatewayMock.Object,
            executeAsyncDelegate);
    }

    protected static TRequest CreateAccountingIdentificationRequest<TRequest>(
        Fixture fixture,
        int? accountingNumber = null,
        ISecurityContext? securityContext = null)
        where TRequest : AccountingIdentificationRequestBase
    {
        accountingNumber ??= fixture.Create<int>();
        securityContext ??= fixture.CreateSecurityContext();

        return (TRequest)Activator.CreateInstance(typeof(TRequest), Guid.NewGuid(), accountingNumber, securityContext)!;
    }

    #endregion

    #region Nested classes

    private class TestAccountingIdentificationCommandFeature<TRequest> : AccountingIdentificationFeatureBase<TRequest>
        where TRequest : AccountingIdentificationRequestBase
    {
        #region Private variables

        private readonly Func<TRequest, CancellationToken, Task>? _executeAsyncDelegate;

        #endregion

        #region Constructor

        public TestAccountingIdentificationCommandFeature(
            IPermissionChecker permissionChecker,
            IAccountingGateway accountingGateway,
            Func<TRequest, CancellationToken, Task>? executeAsyncDelegate = null)
            : base(permissionChecker, accountingGateway)
        {
            _executeAsyncDelegate = executeAsyncDelegate;
        }

        #endregion

        #region Methods

        public override Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
        {
            return _executeAsyncDelegate?.Invoke(request, cancellationToken) ?? Task.CompletedTask;
        }

        #endregion
    }

    #endregion
}