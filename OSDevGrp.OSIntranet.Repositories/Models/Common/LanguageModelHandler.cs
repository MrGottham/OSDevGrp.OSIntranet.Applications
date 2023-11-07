using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
	internal class LanguageModelHandler : GenericCategoryModelHandlerBase<ILanguage, LanguageModel>
    {
        #region Constructor

        public LanguageModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<LanguageModel> Entities => DbContext.Languages;

        #endregion

        #region Methods

        protected override Expression<Func<LanguageModel, bool>> EntitySelector(int primaryKey) => languageModel => languageModel.LanguageIdentifier == primaryKey;

        protected override async Task<bool> CanDeleteAsync(LanguageModel languageModel)
        {
            NullGuard.NotNull(languageModel, nameof(languageModel));

            if (languageModel.Movies != null && languageModel.Movies.Any())
            {
	            return false;
            }

            if (languageModel.Books != null && languageModel.Books.Any())
            {
	            return false;
            }

            if (await DbContext.Movies.FirstOrDefaultAsync(movieModel => movieModel.SpokenLanguageIdentifier != null && movieModel.SpokenLanguageIdentifier == languageModel.LanguageIdentifier) != null)
            {
	            return false;
            }

            return await DbContext.Books.FirstOrDefaultAsync(bookModel => bookModel.WrittenLanguageIdentifier != null && bookModel.WrittenLanguageIdentifier == languageModel.LanguageIdentifier) == null;
        }

		#endregion
	}
}