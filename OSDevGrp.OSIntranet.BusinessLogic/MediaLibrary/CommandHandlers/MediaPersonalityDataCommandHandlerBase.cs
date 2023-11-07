using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class MediaPersonalityDataCommandHandlerBase<TMediaPersonalityDataCommand> : MediaPersonalityIdentificationCommandHandlerBase<TMediaPersonalityDataCommand> where TMediaPersonalityDataCommand : IMediaPersonalityDataCommand
	{
		#region Constructor

		protected MediaPersonalityDataCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected sealed override async Task ManageAsync(TMediaPersonalityDataCommand command)
		{
			NullGuard.NotNull(command, nameof(command));

			IMediaPersonality mediaPersonality = await command.ToDomainAsync(MediaLibraryRepository, CommonRepository);

			await ManageAsync(mediaPersonality);
		}

		protected abstract Task ManageAsync(IMediaPersonality mediaPersonality);

		#endregion
	}
}