﻿using Microsoft.EntityFrameworkCore;
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
	internal class MovieGenreModelHandler : GenericCategoryModelHandlerBase<IMovieGenre, MovieGenreModel>
    {
        #region Methods

        public MovieGenreModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<MovieGenreModel> Entities => DbContext.MovieGenres;

        #endregion

        #region Methods

        protected override Expression<Func<MovieGenreModel, bool>> EntitySelector(int primaryKey) => movieGenreModel => movieGenreModel.MovieGenreIdentifier == primaryKey;

        protected override async Task<bool> CanDeleteAsync(MovieGenreModel movieGenreModel)
        {
            NullGuard.NotNull(movieGenreModel, nameof(movieGenreModel));

            if (movieGenreModel.Movies != null)
            {
	            return movieGenreModel.Movies.Any() == false;
            }

            return await DbContext.Movies.FirstOrDefaultAsync(movieModel => movieModel.MovieGenreIdentifier == movieGenreModel.MovieGenreIdentifier) == null;
        }

		#endregion
	}
}