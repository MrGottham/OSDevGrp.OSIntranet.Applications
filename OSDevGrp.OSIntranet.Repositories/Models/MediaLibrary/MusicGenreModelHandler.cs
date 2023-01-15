﻿using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class MusicGenreModelHandler : GenericCategoryModelHandlerBase<IMusicGenre, MusicGenreModel>
    {
        #region Methods

        public MusicGenreModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<MusicGenreModel> Entities => DbContext.MusicGenres;

        #endregion

        #region Methods

        protected override Expression<Func<MusicGenreModel, bool>> EntitySelector(int primaryKey) => musicGenreModel => musicGenreModel.MusicGenreIdentifier == primaryKey;

        protected override Task<bool> CanDeleteAsync(MusicGenreModel musicGenreModel)
        {
            NullGuard.NotNull(musicGenreModel, nameof(musicGenreModel));

            return Task.FromResult(true);
        }

        #endregion
    }
}