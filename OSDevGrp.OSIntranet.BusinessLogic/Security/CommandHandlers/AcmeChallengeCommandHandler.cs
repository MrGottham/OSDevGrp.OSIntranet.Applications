using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class AcmeChallengeCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<IAcmeChallengeCommand, byte[]>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IAcmeChallengeResolver _acmeChallengeResolver;

        #endregion

        #region Constructor

        public AcmeChallengeCommandHandler(IValidator validator, IAcmeChallengeResolver acmeChallengeResolver)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(acmeChallengeResolver, nameof(acmeChallengeResolver));

            _validator = validator;
            _acmeChallengeResolver = acmeChallengeResolver;
        }

        #endregion

        #region Methods

        public Task<byte[]> ExecuteAsync(IAcmeChallengeCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return Task.Run(() =>
            {
                command.Validate(_validator);

                string constructedKeyAuthorization = _acmeChallengeResolver.GetConstructedKeyAuthorization(command.ChallengeToken);
                if (string.IsNullOrWhiteSpace(constructedKeyAuthorization))
                {
                    throw new IntranetExceptionBuilder(ErrorCode.CannotRetrieveAcmeChallengeForToken).Build();
                }

                return Encoding.UTF8.GetBytes(constructedKeyAuthorization);
            });
        }

        #endregion
    }
}