using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class BorrowerIdentificationCommandHandlerBase<TBorrowerIdentificationCommand> : MediaLibraryCommandHandlerBase<TBorrowerIdentificationCommand> where TBorrowerIdentificationCommand : IBorrowerIdentificationCommand
	{
		#region Constructor

		protected BorrowerIdentificationCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion
	}
}