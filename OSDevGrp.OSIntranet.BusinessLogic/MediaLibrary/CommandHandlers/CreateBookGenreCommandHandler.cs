using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class CreateBookGenreCommandHandler : CreateGenericCategoryCommandHandlerBase<ICreateBookGenreCommand, IBookGenre>
	{
		#region Private variables

		private readonly IMediaLibraryRepository _mediaLibraryRepository;

		#endregion

		#region Constructor

		public CreateBookGenreCommandHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			_mediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Methods

		protected override Task<IBookGenre> GetGenericCategoryAsync(int number) => _mediaLibraryRepository.GetBookGenreAsync(number);

		protected override Task ManageRepositoryAsync(IBookGenre bookGenre)
		{
			NullGuard.NotNull(bookGenre, nameof(bookGenre));

			return _mediaLibraryRepository.CreateBookGenreAsync(bookGenre);
		}

		#endregion
	}
}