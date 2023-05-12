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
	internal class BookModelHandler : MediaModelHandlerBase<IBook, BookModel, BookBindingModel>
	{
		#region Private variables

		private readonly bool _includeBookBindings;

		#endregion

		#region Constructor

		public BookModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeCoreData, bool includeBookBindings, bool includeLendings) 
			: base(dbContext, modelConverter, includeCoreData, includeLendings)
		{
			_includeBookBindings = includeBookBindings;
		}

		#endregion

		#region Properties

		protected sealed override DbSet<BookModel> Entities => DbContext.Books;

		#endregion

		#region Methods

		protected sealed override Expression<Func<LendingModel, bool>> LendingModelsSelector(BookModel bookModel) => lendingModel => lendingModel.BookIdentifier != null && lendingModel.BookIdentifier == bookModel.BookIdentifier;

		protected sealed override async Task<BookModel> OnCreateAsync(IBook book, BookModel bookModel)
		{
			NullGuard.NotNull(book, nameof(book))
				.NotNull(bookModel, nameof(bookModel));

			bookModel = await base.OnCreateAsync(book, bookModel);
			bookModel.CoreData.Book = bookModel;

			bookModel.BookGenre = await DbContext.BookGenres.SingleAsync(bookGenreModel => bookGenreModel.BookGenreIdentifier == book.BookGenre.Number);
			bookModel.WrittenLanguage = book.WrittenLanguage != null
				? await DbContext.Languages.SingleAsync(languageModel => languageModel.LanguageIdentifier == book.WrittenLanguage.Number)
				: null;

			return bookModel;
		}

		protected sealed override async Task<BookModel> OnReadAsync(BookModel bookModel)
		{
			NullGuard.NotNull(bookModel, nameof(bookModel));

			foreach (BookBindingModel bookBindingModel in bookModel.BookBindings ?? new List<BookBindingModel>(0))
			{
				bookBindingModel.MediaPersonality = await MediaPersonalityModelHandler.ReadAsync(bookBindingModel.MediaPersonality);
				bookBindingModel.Deletable = true;
			}

			return await base.OnReadAsync(bookModel);
		}

		protected sealed override async Task OnUpdateAsync(IBook book, BookModel bookModel)
		{
			NullGuard.NotNull(book, nameof(book))
				.NotNull(bookModel, nameof(bookModel));

			await base.OnUpdateAsync(book, bookModel);

			bookModel.BookGenreIdentifier = book.BookGenre.Number;
			bookModel.BookGenre = await DbContext.BookGenres.SingleAsync(bookGenreModel => bookGenreModel.BookGenreIdentifier == book.BookGenre.Number);
			bookModel.WrittenLanguageIdentifier = book.WrittenLanguage?.Number;
			bookModel.WrittenLanguage = book.WrittenLanguage != null
				? await DbContext.Languages.SingleAsync(languageModel => languageModel.LanguageIdentifier == book.WrittenLanguage.Number)
				: null;
			bookModel.InternationalStandardBookNumber = book.InternationalStandardBookNumber;
		}

		protected sealed override async Task<BookModel> OnDeleteAsync(BookModel bookModel)
		{
			NullGuard.NotNull(bookModel, nameof(bookModel));

			while (bookModel.BookBindings.Any())
			{
				EntityEntry<BookBindingModel> deletedEntityEntry = DbContext.BookBindings.Remove(bookModel.BookBindings.First());

				bookModel.BookBindings.Remove(deletedEntityEntry.Entity);
			}

			return await base.OnDeleteAsync(bookModel);
		}

		protected sealed override IQueryable<BookModel> CreateReader()
		{
			IQueryable<BookModel> reader = base.CreateReader()
				.Include(m => m.BookGenre)
				.Include(m => m.WrittenLanguage);

			if (_includeBookBindings == false)
			{
				return reader;
			}

			return reader.Include(m => m.BookBindings)
				.ThenInclude(m => m.MediaPersonality)
				.ThenInclude(m => m.Nationality);
		}

		protected sealed override List<BookBindingModel> GetMediaBindingModels(BookModel bookModel) => bookModel.BookBindings;

		protected sealed override BookBindingModel BuildMediaBindingModel(BookModel bookModel, MediaPersonalityModel mediaPersonalityModel, short role)
		{
			NullGuard.NotNull(bookModel, nameof(bookModel))
				.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			return new BookBindingModel
			{
				BookIdentifier = bookModel.BookIdentifier,
				Book = bookModel,
				MediaPersonalityIdentifier = mediaPersonalityModel.MediaPersonalityIdentifier,
				MediaPersonality = mediaPersonalityModel,
				Role = role
			};
		}

		#endregion
	}
}