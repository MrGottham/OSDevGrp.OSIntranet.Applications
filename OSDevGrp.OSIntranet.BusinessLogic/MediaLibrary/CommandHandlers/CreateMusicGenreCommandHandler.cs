﻿using OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers
{
	internal class CreateMusicGenreCommandHandler : CreateGenericCategoryCommandHandlerBase<ICreateMusicGenreCommand, IMusicGenre>
	{
		#region Private variables

		private readonly IClaimResolver _claimResolver;
		private readonly IMediaLibraryRepository _mediaLibraryRepository;

		#endregion

		#region Constructor

		public CreateMusicGenreCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator)
		{
			NullGuard.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			_claimResolver = claimResolver;
			_mediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Methods

		protected override bool HasNecessaryPermission() => _claimResolver.IsMediaLibraryModifier();

		protected override Task<IMusicGenre> GetGenericCategoryAsync(int number) => _mediaLibraryRepository.GetMusicGenreAsync(number);

		protected override Task ManageRepositoryAsync(IMusicGenre musicGenre)
		{
			NullGuard.NotNull(musicGenre, nameof(musicGenre));

			return _mediaLibraryRepository.CreateMusicGenreAsync(musicGenre);
		}

		#endregion
	}
}