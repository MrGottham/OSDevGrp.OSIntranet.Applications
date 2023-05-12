using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MovieModelHandler : MediaModelHandlerBase<IMovie, MovieModel, MovieBindingModel>
	{
		#region Private variables

		private readonly bool _includeMovieBindings;

		#endregion

		#region Constructor

		public MovieModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeCoreData, bool includeMovieBindings, bool includeLendings)
			: base(dbContext, modelConverter, includeCoreData, includeLendings)
		{
			_includeMovieBindings = includeMovieBindings;
		}

		#endregion

		#region Properties

		protected sealed override DbSet<MovieModel> Entities => DbContext.Movies;

		#endregion

		#region Methods

		protected sealed override Expression<Func<LendingModel, bool>> LendingModelsSelector(MovieModel movieModel) => lendingModel => lendingModel.MovieIdentifier != null && lendingModel.MovieIdentifier == movieModel.MovieIdentifier;

		protected sealed override async Task<MovieModel> OnCreateAsync(IMovie movie, MovieModel movieModel)
		{
			NullGuard.NotNull(movie, nameof(movie))
				.NotNull(movieModel, nameof(movieModel));

			movieModel = await base.OnCreateAsync(movie, movieModel);
			movieModel.CoreData.Movie = movieModel;

			movieModel.MovieGenre = await DbContext.MovieGenres.SingleAsync(movieGenreModel => movieGenreModel.MovieGenreIdentifier == movie.MovieGenre.Number);
			movieModel.SpokenLanguage = movie.SpokenLanguage != null
				? await DbContext.Languages.SingleAsync(languageModel => languageModel.LanguageIdentifier == movie.SpokenLanguage.Number)
				: null;

			return movieModel;
		}

		protected sealed override async Task<MovieModel> OnReadAsync(MovieModel movieModel)
		{
			NullGuard.NotNull(movieModel, nameof(movieModel));

			foreach (MovieBindingModel movieBindingModel in movieModel.MovieBindings ?? new List<MovieBindingModel>(0))
			{
				movieBindingModel.MediaPersonality = await MediaPersonalityModelHandler.ReadAsync(movieBindingModel.MediaPersonality);
				movieBindingModel.Deletable = true;
			}

			return await base.OnReadAsync(movieModel);
		}

		protected sealed override async Task OnUpdateAsync(IMovie movie, MovieModel movieModel)
		{
			NullGuard.NotNull(movie, nameof(movie))
				.NotNull(movieModel, nameof(movieModel));

			await base.OnUpdateAsync(movie, movieModel);

			movieModel.MovieGenreIdentifier = movie.MovieGenre.Number;
			movieModel.MovieGenre = await DbContext.MovieGenres.SingleAsync(movieGenreModel => movieGenreModel.MovieGenreIdentifier == movie.MovieGenre.Number);
			movieModel.SpokenLanguageIdentifier = movie.SpokenLanguage?.Number;
			movieModel.SpokenLanguage = movie.SpokenLanguage != null
				? await DbContext.Languages.SingleAsync(languageModel => languageModel.LanguageIdentifier == movie.SpokenLanguage.Number)
				: null;
			movieModel.Length = movie.Length;
		}

		protected sealed override async Task<MovieModel> OnDeleteAsync(MovieModel movieModel)
		{
			NullGuard.NotNull(movieModel, nameof(movieModel));

			while (movieModel.MovieBindings.Any())
			{
				EntityEntry<MovieBindingModel> deletedEntityEntry = DbContext.MovieBindings.Remove(movieModel.MovieBindings.First());

				movieModel.MovieBindings.Remove(deletedEntityEntry.Entity);
			}

			return await base.OnDeleteAsync(movieModel);
		}

		protected sealed override IQueryable<MovieModel> CreateReader()
		{
			IQueryable<MovieModel> reader = base.CreateReader()
				.Include(m => m.MovieGenre)
				.Include(m => m.SpokenLanguage);

			if (_includeMovieBindings == false)
			{
				return reader;
			}

			return reader.Include(m => m.MovieBindings)
				.ThenInclude(m => m.MediaPersonality)
				.ThenInclude(m => m.Nationality);
		}

		protected sealed override List<MovieBindingModel> GetMediaBindingModels(MovieModel movieModel) => movieModel.MovieBindings;

		protected sealed override MovieBindingModel BuildMediaBindingModel(MovieModel movieModel, MediaPersonalityModel mediaPersonalityModel, short role)
		{
			NullGuard.NotNull(movieModel, nameof(movieModel))
				.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			return new MovieBindingModel
			{
				MovieIdentifier = movieModel.MovieIdentifier,
				Movie = movieModel,
				MediaPersonalityIdentifier = mediaPersonalityModel.MediaPersonalityIdentifier,
				MediaPersonality = mediaPersonalityModel,
				Role = role
			};
		}

		#endregion
	}
}