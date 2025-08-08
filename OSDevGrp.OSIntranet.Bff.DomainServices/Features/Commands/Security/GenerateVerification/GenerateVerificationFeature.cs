using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Security.GenerateVerification;

internal class GenerateVerificationFeature : ICommandFeature<GenerateVerificationRequest>, IPermissionVerifiable<GenerateVerificationRequest>
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly IHashGenerator _hashGenerator;
    private readonly IVerificationCodeGenerator _verificationCodeGenerator;
    private readonly IVerificationCodeStorage _verificationCodeStorage;
    private readonly ICaptchaGenerator _captchaGenerator;
    private readonly TimeProvider _timeProvider;

    #endregion

    #region Constructor

    public GenerateVerificationFeature(IPermissionChecker permissionChecker, IHashGenerator hashGenerator, IVerificationCodeGenerator verificationCodeGenerator, IVerificationCodeStorage verificationCodeStorage, ICaptchaGenerator captchaGenerator, TimeProvider timeProvider)
    {
        _permissionChecker = permissionChecker;
        _hashGenerator = hashGenerator;
        _verificationCodeGenerator = verificationCodeGenerator;
        _verificationCodeStorage = verificationCodeStorage;
        _captchaGenerator = captchaGenerator;
        _timeProvider = timeProvider;
    }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, GenerateVerificationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => VerifyPermission(securityContext.User), cancellationToken);
    }

    public async Task ExecuteAsync(GenerateVerificationRequest request, CancellationToken cancellationToken = default)
    {
        string verificationKey = await _hashGenerator.GenerateAsync(Encoding.UTF8.GetBytes(request.RequestId.ToString("D")), cancellationToken);

        string verificationCode = await _verificationCodeGenerator.GenerateAsync(cancellationToken);
        byte[] captchaImage = await _captchaGenerator.GenerateAsync(verificationCode, cancellationToken);

        DateTimeOffset expires = _timeProvider.GetUtcNow().AddSeconds(150);
        await _verificationCodeStorage.StoreVerificationCodeAsync(verificationKey, verificationCode, expires, cancellationToken);

        request.OnVerificationCreated(verificationKey, captchaImage, expires);
    }

    private bool VerifyPermission(ClaimsPrincipal user)
    {
        return _permissionChecker.IsAuthenticated(user);
    }

    #endregion
}