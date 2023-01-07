using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System.Text;

namespace OSDevGrp.OSIntranet.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        #region Private variables

        private readonly IClaimResolver _claimResolver;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IAcmeChallengeResolver _acmeChallengeResolver;
        private readonly IConverter _securityModelConverter = new SecurityModelConverter();

        #endregion

        #region Constructor

        public SecurityController(IClaimResolver claimResolver, IDataProtectionProvider dataProtectionProvider, IAcmeChallengeResolver acmeChallengeResolver)
        {
            NullGuard.NotNull(claimResolver, nameof(claimResolver))
                .NotNull(dataProtectionProvider, nameof(dataProtectionProvider))
                .NotNull(acmeChallengeResolver, nameof(acmeChallengeResolver));

            _claimResolver = claimResolver;
            _dataProtectionProvider = dataProtectionProvider;
            _acmeChallengeResolver = acmeChallengeResolver;
        }

        #endregion

        #region Methods

        [Authorize(Policy = Policies.AcquireTokenPolicy)]
        [HttpPost("/api/oauth/token")]
        public ActionResult<AccessTokenModel> AcquireToken([FromForm(Name = "grant_type")]string grantType)
        {
            if (string.IsNullOrWhiteSpace(grantType))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(grantType))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(grantType))
                    .Build();
            }

            IToken token = _claimResolver.GetToken<IToken>(UnprotectBase64Token);

            if (string.CompareOrdinal(grantType, "client_credentials") != 0 || token == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser).Build();
            }

            return Ok(_securityModelConverter.Convert<IToken, AccessTokenModel>(token));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/.well-known/acme-challenge/{challengeToken}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public IActionResult AcmeChallenge(string challengeToken)
        {
            if (string.IsNullOrWhiteSpace(challengeToken))
            {
                throw new IntranetExceptionBuilder(ErrorCode.ValueCannotBeNullOrWhiteSpace, nameof(challengeToken))
                    .WithValidatingType(typeof(string))
                    .WithValidatingField(nameof(challengeToken))
                    .Build();
            }

            string constructedKeyAuthorization = _acmeChallengeResolver.GetConstructedKeyAuthorization(challengeToken);
            if (string.IsNullOrWhiteSpace(constructedKeyAuthorization))
            {
                throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveAcmeChallengeForToken).Build();
            }

            return File(Encoding.UTF8.GetBytes(constructedKeyAuthorization), "application/octet-stream");
        }

        private string UnprotectBase64Token(string protectedBase64Token)
        {
            NullGuard.NotNullOrWhiteSpace(protectedBase64Token, nameof(protectedBase64Token));

            IDataProtector dataProtector = _dataProtectionProvider.CreateProtector("TokenProtection");

            return dataProtector.Unprotect(protectedBase64Token);
        }

        #endregion
    }
}