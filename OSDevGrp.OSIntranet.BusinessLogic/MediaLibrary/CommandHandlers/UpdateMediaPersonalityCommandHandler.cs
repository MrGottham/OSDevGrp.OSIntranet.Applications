using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class UpdateMediaPersonalityCommandHandler : MediaPersonalityDataCommandHandlerBase<IUpdateMediaPersonalityCommand>
	{
		#region Constructor

		public UpdateMediaPersonalityCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
			: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task ManageAsync(IMediaPersonality mediaPersonality)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality));

			return MediaLibraryRepository.UpdateMediaPersonalityAsync(mediaPersonality);
		}

		#endregion
	}
}