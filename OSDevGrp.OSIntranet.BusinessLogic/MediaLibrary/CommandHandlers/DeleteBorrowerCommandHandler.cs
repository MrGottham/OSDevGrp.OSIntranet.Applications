using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class DeleteBorrowerCommandHandler : BorrowerIdentificationCommandHandlerBase<IDeleteBorrowerCommand>
	{
		#region Constructor

		public DeleteBorrowerCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task ManageAsync(IDeleteBorrowerCommand deleteBorrowerCommand)
		{
			NullGuard.NotNull(deleteBorrowerCommand, nameof(deleteBorrowerCommand));

			return MediaLibraryRepository.DeleteBorrowerAsync(deleteBorrowerCommand.BorrowerIdentifier);
		}

		#endregion
	}
}