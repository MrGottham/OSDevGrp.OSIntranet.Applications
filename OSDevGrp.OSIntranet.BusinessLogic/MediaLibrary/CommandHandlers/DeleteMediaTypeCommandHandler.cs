using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class DeleteMediaTypeCommandHandler : DeleteGenericCategoryCommandHandlerBase<IDeleteMediaTypeCommand, IMediaType>
	{
		#region Private variables

		private readonly IMediaLibraryRepository _mediaLibraryRepository;

		#endregion

		#region Constructor

		public DeleteMediaTypeCommandHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			_mediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Methods

		protected override Task<IMediaType> GetGenericCategoryAsync(int number) => _mediaLibraryRepository.GetMediaTypeAsync(number);

		protected override Task ManageRepositoryAsync(int number) => _mediaLibraryRepository.DeleteMediaTypeAsync(number);

		#endregion
	}
}