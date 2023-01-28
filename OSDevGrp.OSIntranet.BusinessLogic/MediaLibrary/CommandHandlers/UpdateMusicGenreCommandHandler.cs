using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class UpdateMusicGenreCommandHandler : UpdateGenericCategoryCommandHandlerBase<IUpdateMusicGenreCommand, IMusicGenre>
	{
		#region Private variables

		private readonly IMediaLibraryRepository _mediaLibraryRepository;

		#endregion

		#region Constructor

		public UpdateMusicGenreCommandHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			_mediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Methods

		protected override Task<IMusicGenre> GetGenericCategoryAsync(int number) => _mediaLibraryRepository.GetMusicGenreAsync(number);

		protected override Task ManageRepositoryAsync(IMusicGenre musicGenre)
		{
			NullGuard.NotNull(musicGenre, nameof(musicGenre));

			return _mediaLibraryRepository.UpdateMusicGenreAsync(musicGenre);
		}

		#endregion
	}
}