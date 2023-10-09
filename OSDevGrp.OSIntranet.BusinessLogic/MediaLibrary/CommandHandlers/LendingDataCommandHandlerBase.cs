using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class LendingDataCommandHandlerBase<TLendingDataCommand> : LendingIdentificationCommandHandlerBase<TLendingDataCommand> where TLendingDataCommand : ILendingDataCommand
	{
		#region Constructor

		protected LendingDataCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected sealed override async Task ManageAsync(TLendingDataCommand command)
		{
			NullGuard.NotNull(command, nameof(command));

			ILending lending = await command.ToDomainAsync(MediaLibraryRepository, CommonRepository);

			await ManageAsync(lending);
		}

		protected abstract Task ManageAsync(ILending lending);

		#endregion
	}
}