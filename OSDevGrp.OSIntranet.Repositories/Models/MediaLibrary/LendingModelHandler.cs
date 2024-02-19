using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class LendingModelHandler : ModelHandlerBase<ILending, RepositoryContext, LendingModel, Guid>
	{
		#region Private variables

		private readonly bool _includeBorrower;
		private readonly bool _includeMedia;

		#endregion

		#region Constructor

		public LendingModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeBorrower, bool includeMedia) 
			: base(dbContext, modelConverter)
		{
			_includeBorrower = includeBorrower;
			_includeMedia = includeMedia;
		}

		#endregion

		#region Properties

		protected sealed override DbSet<LendingModel> Entities => DbContext.Lendings;

		protected sealed override Func<ILending, Guid> PrimaryKey => lending => lending.LendingIdentifier;

		protected sealed override IQueryable<LendingModel> Reader => CreateReader();

		#endregion

		#region Methods

		internal async Task<IEnumerable<LendingModel>> ReadAsync(IEnumerable<LendingModel> lendingModels)
		{
			NullGuard.NotNull(lendingModels, nameof(lendingModels));

			return await Task.WhenAll(lendingModels.Select(OnReadAsync).ToArray());
		}

		internal Task<List<LendingModel>> DeleteAsync(List<LendingModel> lendingModels)
		{
			NullGuard.NotNull(lendingModels, nameof(lendingModels));

			while (lendingModels.Any())
			{
				EntityEntry<LendingModel> deletedEntityEntry = DbContext.Lendings.Remove(lendingModels.First());

				lendingModels.Remove(deletedEntityEntry.Entity);
			}

			return Task.FromResult(lendingModels);
		}

		internal Expression<Func<LendingModel, bool>> IsDeletable => lendingModel => lendingModel.ReturnedDate != null && lendingModel.ReturnedDate <= DateTime.Today;

		protected sealed override Expression<Func<LendingModel, bool>> EntitySelector(Guid primaryKey) => lendingModel => lendingModel.ExternalLendingIdentifier == primaryKey;

		protected sealed override Task<IEnumerable<ILending>> SortAsync(IEnumerable<ILending> lendingCollection)
		{
			NullGuard.NotNull(lendingCollection, nameof(lendingCollection));

			return Task.FromResult(lendingCollection.OrderByDescending(lending => lending.LendingDate).ThenByDescending(lending => lending.RecallDate).AsEnumerable());
		}

		protected sealed override async Task<LendingModel> OnCreateAsync(ILending lending, LendingModel lendingModel)
		{
			NullGuard.NotNull(lending, nameof(lending))
				.NotNull(lendingModel, nameof(lendingModel));

			await ApplyAsync(lendingModel, lending.Borrower);
			await ApplyAsync(lendingModel, lending.Media);

			return lendingModel;
		}

		protected sealed override async Task<LendingModel> OnReadAsync(LendingModel lendingModel)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel));

			lendingModel.Deletable = await CanDeleteAsync(lendingModel);

			return lendingModel;
		}

		protected override async Task OnUpdateAsync(ILending lending, LendingModel lendingModel)
		{
			NullGuard.NotNull(lending, nameof(lending))
				.NotNull(lendingModel, nameof(lendingModel));

			await ApplyAsync(lendingModel, lending.Borrower);
			await ApplyAsync(lendingModel, lending.Media);

			lendingModel.LendingDate = lending.LendingDate;
			lendingModel.RecallDate = lending.RecallDate;
			lendingModel.ReturnedDate = lending.ReturnedDate;
		}

		protected sealed override Task<bool> CanDeleteAsync(LendingModel lendingModel)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel));

			return Task.FromResult(IsDeletable.Compile().Invoke(lendingModel));
		}

		private IQueryable<LendingModel> CreateReader()
		{
			IQueryable<LendingModel> reader = Entities;

			if (_includeBorrower)
			{
				reader = reader.Include(m => m.Borrower);
			}

			if (_includeMedia)
			{
				reader = reader.Include(m => m.Movie).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Movie).ThenInclude(m => m.MovieGenre)
					.Include(m => m.Movie).ThenInclude(m => m.SpokenLanguage)
					.Include(m => m.Music).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Music).ThenInclude(m => m.MusicGenre)
					.Include(m => m.Book).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Book).ThenInclude(m => m.BookGenre)
					.Include(m => m.Book).ThenInclude(m => m.WrittenLanguage);
			}

			return reader;
		}

		private Task<BorrowerModel> GetBorrowerModelAsync(IBorrower borrower)
		{
			NullGuard.NotNull(borrower, nameof(borrower));

			return DbContext.Borrowers.SingleAsync(m => m.ExternalBorrowerIdentifier == borrower.BorrowerIdentifier);
		}

		private Task<MovieModel> GetMovieModelAsync(IMedia media)
		{
			NullGuard.NotNull(media, nameof(media));

			return DbContext.Movies.SingleAsync(m => m.ExternalMediaIdentifier == media.MediaIdentifier);
		}

		private Task<MusicModel> GetMusicModelAsync(IMedia media)
		{
			NullGuard.NotNull(media, nameof(media));

			return DbContext.Music.SingleAsync(m => m.ExternalMediaIdentifier == media.MediaIdentifier);
		}

		private Task<BookModel> GetBookModelAsync(IMedia media)
		{
			NullGuard.NotNull(media, nameof(media));

			return DbContext.Books.SingleAsync(m => m.ExternalMediaIdentifier == media.MediaIdentifier);
		}

		private async Task ApplyAsync(LendingModel lendingModel, IBorrower borrower)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(borrower, nameof(borrower));

			BorrowerModel borrowerModel = await GetBorrowerModelAsync(borrower);
			lendingModel.BorrowerIdentifier = borrowerModel.BorrowerIdentifier;
			lendingModel.Borrower = borrowerModel;
		}

		private async Task ApplyAsync(LendingModel lendingModel, IMedia media)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(media, nameof(media));

			if (media is IMovie)
			{
				Apply(lendingModel, await GetMovieModelAsync(media));
				return;
			}

			if (media is IMusic)
			{
				Apply(lendingModel, await GetMusicModelAsync(media));
				return;
			}

			if (media is IBook)
			{
				Apply(lendingModel, await GetBookModelAsync(media));
				return;
			}

			throw new NotSupportedException($"{media.GetType()} is not supported as a media.");
		}

		private static void Apply(LendingModel lendingModel, MovieModel movieModel)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(movieModel, nameof(movieModel));

			lendingModel.MovieIdentifier = movieModel.MovieIdentifier;
			lendingModel.Movie = movieModel;
			lendingModel.MusicIdentifier = null;
			lendingModel.Music = null;
			lendingModel.BookIdentifier = null;
			lendingModel.Book = null;
		}

		private static void Apply(LendingModel lendingModel, MusicModel musicModel)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(musicModel, nameof(musicModel));

			lendingModel.MovieIdentifier = null;
			lendingModel.Movie = null;
			lendingModel.MusicIdentifier = musicModel.MusicIdentifier;
			lendingModel.Music = musicModel;
			lendingModel.BookIdentifier = null;
			lendingModel.Book = null;
		}

		private static void Apply(LendingModel lendingModel, BookModel bookModel)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(bookModel, nameof(bookModel));

			lendingModel.MovieIdentifier = null;
			lendingModel.Movie = null;
			lendingModel.MusicIdentifier = null;
			lendingModel.Music = null;
			lendingModel.BookIdentifier = bookModel.BookIdentifier;
			lendingModel.Book = bookModel;
		}

		#endregion
	}
}