﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal abstract class MediaModelHandlerBase<TMedia, TMediaModel> : ModelHandlerBase<TMedia, RepositoryContext, TMediaModel, Guid> where TMedia : class, IMedia where TMediaModel : MediaModelBase, new()
    {
        #region Constructor

        protected MediaModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected sealed override Func<TMedia, Guid> PrimaryKey => media => media.MediaIdentifier;

        protected override IQueryable<TMediaModel> Reader => throw new NotImplementedException(); // TODO: Include MediaCoreData.

        protected override IQueryable<TMediaModel> UpdateReader => throw new NotImplementedException(); // TODO: Include MediaCoreData.

        protected override IQueryable<TMediaModel> DeleteReader => throw new NotImplementedException();  // TODO: Include MediaCoreData.

        #endregion

        #region Methods

        protected sealed override Expression<Func<TMediaModel, bool>> EntitySelector(Guid primaryKey) => mediaModel => mediaModel.MediaIdentifier == primaryKey;

        protected override Task<IEnumerable<TMedia>> SortAsync(IEnumerable<TMedia> mediaCollection)
        {
            NullGuard.NotNull(mediaCollection, nameof(mediaCollection));

            return Task.FromResult(mediaCollection.OrderBy(media => media.Name).AsEnumerable());
        }

        protected sealed override Task<TMediaModel> OnCreateAsync(TMedia media, TMediaModel mediaModel)
        {
            NullGuard.NotNull(media, nameof(media))
                .NotNull(mediaModel, nameof(mediaModel));

            throw new NotImplementedException(); // Handle Creation for Media and MediaCoreData.
        }

        protected sealed override async Task<TMediaModel> OnReadAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            mediaModel.Deletable = await CanDeleteAsync(mediaModel);

            return mediaModel;
        }

        protected override Task OnUpdateAsync(TMedia media, TMediaModel mediaModel)
        {
            NullGuard.NotNull(media, nameof(media))
                .NotNull(mediaModel, nameof(mediaModel));

            mediaModel.CoreData.Name = media.Name;
            mediaModel.CoreData.Description = media.Description;

            string imageAsBase64 = Encoding.UTF8.GetString(media.Image ?? Array.Empty<byte>());
            mediaModel.CoreData.Image = string.IsNullOrWhiteSpace(imageAsBase64) ? null : imageAsBase64;

            return Task.CompletedTask;
        }

        protected override Task<bool> CanDeleteAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            return Task.FromResult(false);
        }

        protected sealed override Task<TMediaModel> OnDeleteAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            throw new NotImplementedException(); // TODO: Handle Deletion of Media and MediaCoreData.
        }

        #endregion
    }
}