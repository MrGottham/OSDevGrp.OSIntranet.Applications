using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class MediaPersonalityIdentificationCommandHandlerBase<TMediaPersonalityIdentificationCommand> : MediaLibraryCommandHandlerBase<TMediaPersonalityIdentificationCommand> where TMediaPersonalityIdentificationCommand : IMediaPersonalityIdentificationCommand
	{
		#region Constructor

		protected MediaPersonalityIdentificationCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion
	}
}