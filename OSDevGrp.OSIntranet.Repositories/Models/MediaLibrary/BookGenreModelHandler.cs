using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class BookGenreModelHandler : GenericCategoryModelHandlerBase<IBookGenre, BookGenreModel>
    {
        #region Methods

        public BookGenreModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<BookGenreModel> Entities => DbContext.BookGenres;

        #endregion

        #region Methods

        protected override Expression<Func<BookGenreModel, bool>> EntitySelector(int primaryKey) => bookGenreModel => bookGenreModel.BookGenreIdentifier == primaryKey;

        protected override async Task<bool> CanDeleteAsync(BookGenreModel bookGenreModel)
        {
            NullGuard.NotNull(bookGenreModel, nameof(bookGenreModel));

            if (bookGenreModel.Books != null)
            {
	            return bookGenreModel.Books.Any() == false;
            }

            return await DbContext.Books.FirstOrDefaultAsync(bookModel => bookModel.BookGenreIdentifier == bookGenreModel.BookGenreIdentifier) == null;
        }

		#endregion
	}
}