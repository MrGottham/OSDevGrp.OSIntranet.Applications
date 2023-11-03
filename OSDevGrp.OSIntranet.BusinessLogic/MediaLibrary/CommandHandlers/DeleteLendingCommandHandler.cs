using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class DeleteLendingCommandHandler : LendingIdentificationCommandHandlerBase<IDeleteLendingCommand>
	{
		#region Constructor

		public DeleteLendingCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task ManageAsync(IDeleteLendingCommand deleteLendingCommand)
		{
			NullGuard.NotNull(deleteLendingCommand, nameof(deleteLendingCommand));

			return MediaLibraryRepository.DeleteLendingAsync(deleteLendingCommand.LendingIdentifier);
		}

		#endregion
	}
}