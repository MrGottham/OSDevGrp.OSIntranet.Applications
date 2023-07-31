using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class MediaIdentificationCommandHandlerBase<TMediaIdentificationCommand> : MediaLibraryCommandHandlerBase<TMediaIdentificationCommand> where TMediaIdentificationCommand : IMediaIdentificationCommand
	{
		#region Constructors

		protected MediaIdentificationCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion
	}
}