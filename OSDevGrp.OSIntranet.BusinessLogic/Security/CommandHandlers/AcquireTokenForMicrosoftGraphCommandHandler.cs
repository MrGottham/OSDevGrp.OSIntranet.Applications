using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class AcquireTokenForMicrosoftGraphCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<IAcquireTokenForMicrosoftGraphCommand, IRefreshableToken>
    {
        #region Private variables

        private readonly IMicrosoftGraphRepository _microsoftGraphRepository;

        #endregion

        #region Constructor

        public AcquireTokenForMicrosoftGraphCommandHandler(IMicrosoftGraphRepository microsoftGraphRepository)
        {
            NullGuard.NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository));

            _microsoftGraphRepository = microsoftGraphRepository;
        }

        #endregion

        #region Methods

        public Task<IRefreshableToken> ExecuteAsync(IAcquireTokenForMicrosoftGraphCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return _microsoftGraphRepository.AcquireTokenAsync(command.RedirectUri, command.Code);
        }

        #endregion
    }
}
