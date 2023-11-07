using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class DeleteMediaPersonalityCommandHandler : MediaPersonalityIdentificationCommandHandlerBase<IDeleteMediaPersonalityCommand>
	{
		#region Constructor

		public DeleteMediaPersonalityCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task ManageAsync(IDeleteMediaPersonalityCommand deleteMediaPersonalityCommand)
		{
			NullGuard.NotNull(deleteMediaPersonalityCommand, nameof(deleteMediaPersonalityCommand));

			return MediaLibraryRepository.DeleteMediaPersonalityAsync(deleteMediaPersonalityCommand.MediaPersonalityIdentifier);
		}

		#endregion
	}
}