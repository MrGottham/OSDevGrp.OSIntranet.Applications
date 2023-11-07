using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal abstract class MediaLibraryCommandHandlerBase<TMediaLibraryCommand> : CommandHandlerTransactionalBase, ICommandHandler<TMediaLibraryCommand> where TMediaLibraryCommand : IMediaLibraryCommand
	{
		#region Constructor

		protected MediaLibraryCommandHandlerBase(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			Validator = validator;
			ClaimResolver = claimResolver;
			MediaLibraryRepository = mediaLibraryRepository;
			CommonRepository = commonRepository;
		}

		#endregion

		#region Properties

		protected IValidator Validator { get; }

		protected IClaimResolver ClaimResolver { get; }

		protected IMediaLibraryRepository MediaLibraryRepository { get; }

		protected ICommonRepository CommonRepository { get; }

		#endregion

		#region Methods

		public Task ExecuteAsync(TMediaLibraryCommand command)
		{
			NullGuard.NotNull(command, nameof(command));

			command.Validate(Validator, ClaimResolver, MediaLibraryRepository, CommonRepository);

			return ManageAsync(command);
		}

		protected abstract Task ManageAsync(TMediaLibraryCommand command);

		#endregion
	}
}