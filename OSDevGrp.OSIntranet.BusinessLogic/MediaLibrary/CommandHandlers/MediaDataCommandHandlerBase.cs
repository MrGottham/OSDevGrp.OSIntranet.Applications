using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class MediaDataCommandHandlerBase<TMediaDataCommand, TMedia> : MediaIdentificationCommandHandlerBase<TMediaDataCommand> where TMediaDataCommand : IMediaDataCommand<TMedia> where TMedia : IMedia
	{
		#region Constructors

		protected MediaDataCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected sealed override async Task ManageAsync(TMediaDataCommand command)
		{
			NullGuard.NotNull(command, nameof(command));

			TMedia media = await command.ToDomainAsync(MediaLibraryRepository, CommonRepository);

			await ManageAsync(media);
		}

		protected abstract Task ManageAsync(TMedia media);

		#endregion
	}
}