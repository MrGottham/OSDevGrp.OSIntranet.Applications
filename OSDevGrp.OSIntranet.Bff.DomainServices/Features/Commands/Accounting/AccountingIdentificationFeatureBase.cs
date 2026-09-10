using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal abstract class AccountingIdentificationFeatureBase<TAccountingIdentificationRequest> : ICommandFeature<TAccountingIdentificationRequest>, IPermissionVerifiable<TAccountingIdentificationRequest>
    where TAccountingIdentificationRequest : AccountingIdentificationRequestBase
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly IAccountingGateway _accountingGateway;

    #endregion

    #region Constructor

    protected AccountingIdentificationFeatureBase(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway)
    {
        _permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
        _accountingGateway = accountingGateway ?? throw new ArgumentNullException(nameof(accountingGateway));
    }

    #endregion

    #region Properties

    protected IPermissionChecker PermissionChecker => _permissionChecker;

    protected IAccountingGateway AccountingGateway => _accountingGateway;

    #endregion

    #region Methods

    public virtual Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, TAccountingIdentificationRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => VerifyPermission(securityContext.User, request.AccountingNumber), cancellationToken);
    }

    public abstract Task ExecuteAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken);

    private bool VerifyPermission(ClaimsPrincipal user, int accountingNumber)
    {
        return PermissionChecker.IsAuthenticated(user) && PermissionChecker.HasAccountingAccess(user) && PermissionChecker.IsAccountingModifier(user, accountingNumber);
    }

    #endregion
}