using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class DeleteMovieGenreCommandHandler : DeleteGenericCategoryCommandHandlerBase<IDeleteMovieGenreCommand, IMovieGenre>
	{
		#region Private variables

		private readonly IMediaLibraryRepository _mediaLibraryRepository;

		#endregion

		#region Constructor

		public DeleteMovieGenreCommandHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			_mediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Methods

		protected override Task<IMovieGenre> GetGenericCategoryAsync(int number) => _mediaLibraryRepository.GetMovieGenreAsync(number);

		protected override Task ManageRepositoryAsync(int number) => _mediaLibraryRepository.DeleteMovieGenreAsync(number);

		#endregion
	}
}