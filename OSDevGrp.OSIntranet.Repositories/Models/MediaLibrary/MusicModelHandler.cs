using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MusicModelHandler : MediaModelHandlerBase<IMusic, MusicModel, MusicBindingModel>
	{
		#region Private variables

		private readonly bool _includeMusicBindings;

		#endregion

		#region Constructor

		public MusicModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeCoreData, bool includeMusicBindings)
			: base(dbContext, modelConverter, includeCoreData)
		{
			_includeMusicBindings = includeMusicBindings;
		}

		#endregion

		#region Properties

		protected sealed override DbSet<MusicModel> Entities => DbContext.Music;

		#endregion

		#region Methods

		protected sealed override async Task<MusicModel> OnCreateAsync(IMusic music, MusicModel musicModel)
		{
			NullGuard.NotNull(music, nameof(music))
				.NotNull(musicModel, nameof(musicModel));

			musicModel = await base.OnCreateAsync(music, musicModel);
			musicModel.CoreData.Music = musicModel;

			musicModel.MusicGenre = await DbContext.MusicGenres.SingleAsync(musicGenreModel => musicGenreModel.MusicGenreIdentifier == music.MusicGenre.Number);

			return musicModel;
		}

		protected sealed override Task<MusicModel> OnReadAsync(MusicModel musicModel)
		{
			NullGuard.NotNull(musicModel, nameof(musicModel));

			foreach (MusicBindingModel musicBindingModel in musicModel.MusicBindings ?? new List<MusicBindingModel>(0))
			{
				musicBindingModel.Deletable = true;
			}

			return base.OnReadAsync(musicModel);
		}

		protected sealed override async Task OnUpdateAsync(IMusic music, MusicModel musicModel)
		{
			NullGuard.NotNull(music, nameof(music))
				.NotNull(musicModel, nameof(musicModel));

			await base.OnUpdateAsync(music, musicModel);

			musicModel.MusicGenreIdentifier = music.MusicGenre.Number;
			musicModel.MusicGenre = await DbContext.MusicGenres.SingleAsync(musicGenreModel => musicGenreModel.MusicGenreIdentifier == music.MusicGenre.Number);
			musicModel.Tracks = music.Tracks;
		}

		protected sealed override async Task<MusicModel> OnDeleteAsync(MusicModel musicModel)
		{
			NullGuard.NotNull(musicModel, nameof(musicModel));

			while (musicModel.MusicBindings.Any())
			{
				EntityEntry<MusicBindingModel> deletedEntityEntry = DbContext.MusicBindings.Remove(musicModel.MusicBindings.First());

				musicModel.MusicBindings.Remove(deletedEntityEntry.Entity);
			}

			return await base.OnDeleteAsync(musicModel);
		}

		protected sealed override IQueryable<MusicModel> CreateReader()
		{
			IQueryable<MusicModel> reader = base.CreateReader()
				.Include(m => m.MusicGenre);

			if (_includeMusicBindings == false)
			{
				return reader;
			}

			return reader.Include(m => m.MusicBindings)
				.ThenInclude(m => m.MediaPersonality)
				.ThenInclude(m => m.Nationality);
		}

		protected sealed override List<MusicBindingModel> GetMediaBindingModels(MusicModel musicModel) => musicModel.MusicBindings;

		protected sealed override MusicBindingModel BuildMediaBindingModel(MusicModel musicModel, MediaPersonalityModel mediaPersonalityModel, short role)
		{
			NullGuard.NotNull(musicModel, nameof(musicModel))
				.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			return new MusicBindingModel
			{
				MusicIdentifier = musicModel.MusicIdentifier,
				Music = musicModel,
				MediaPersonalityIdentifier = mediaPersonalityModel.MediaPersonalityIdentifier,
				MediaPersonality = mediaPersonalityModel,
				Role = role
			};
		}

		#endregion
	}
}