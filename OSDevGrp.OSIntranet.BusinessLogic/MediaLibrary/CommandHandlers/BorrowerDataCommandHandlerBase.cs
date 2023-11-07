using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class BorrowerDataCommandHandlerBase<TBorrowerDataCommand> : BorrowerIdentificationCommandHandlerBase<TBorrowerDataCommand> where TBorrowerDataCommand : IBorrowerDataCommand
	{
		#region Constructor

		protected BorrowerDataCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected sealed override async Task ManageAsync(TBorrowerDataCommand command)
		{
			NullGuard.NotNull(command, nameof(command));

			IBorrower borrower = await command.ToDomainAsync(MediaLibraryRepository, CommonRepository);

			await ManageAsync(borrower);
		}

		protected abstract Task ManageAsync(IBorrower borrower);

		#endregion
	}
}