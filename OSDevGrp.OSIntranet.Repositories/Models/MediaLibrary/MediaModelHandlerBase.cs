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
	internal abstract class MediaModelHandlerBase<TMedia, TMediaModel, TMediaBindingModel> : ModelHandlerBase<TMedia, RepositoryContext, TMediaModel, Guid> where TMedia : class, IMedia where TMediaModel : MediaModelBase, new() where TMediaBindingModel : MediaBindingModelBase
    {
        #region Private variables

        private readonly bool _includeCoreData;

        #endregion

        #region Constructor

        protected MediaModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter, bool includeCoreData) 
            : base(dbContext, modelConverter)
        {
	        _includeCoreData = includeCoreData;
        }

        #endregion

        #region Properties

        protected sealed override Func<TMedia, Guid> PrimaryKey => media => media.MediaIdentifier;

        protected sealed override IQueryable<TMediaModel> Reader => CreateReader();

        #endregion

        #region Methods

        protected sealed override Expression<Func<TMediaModel, bool>> EntitySelector(Guid primaryKey) => mediaModel => mediaModel.ExternalMediaIdentifier == ValueConverter.GuidToString(primaryKey);

        protected sealed override Task<IEnumerable<TMedia>> SortAsync(IEnumerable<TMedia> mediaCollection)
        {
            NullGuard.NotNull(mediaCollection, nameof(mediaCollection));

            return Task.FromResult(mediaCollection.OrderBy(media => media.ToString()).AsEnumerable());
        }

        protected override async Task<TMediaModel> OnCreateAsync(TMedia media, TMediaModel mediaModel)
        {
            NullGuard.NotNull(media, nameof(media))
                .NotNull(mediaModel, nameof(mediaModel));

            mediaModel.CoreData.MediaType = await DbContext.MediaTypes.SingleAsync(mediaTypeModel => mediaTypeModel.MediaTypeIdentifier == media.MediaType.Number);

            EntityEntry<MediaCoreDataModel> createdEntityEntry = await DbContext.MediaCoreData.AddAsync(mediaModel.CoreData);
            mediaModel.CoreData = createdEntityEntry.Entity;

            await HandleMediaBindingsAsync(media.GetMediaBindings().ToArray(), mediaModel, GetMediaBindingModels(mediaModel));

            return mediaModel;
        }

        protected override async Task<TMediaModel> OnReadAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            mediaModel.Deletable = await CanDeleteAsync(mediaModel);

            return mediaModel;
        }

        protected override async Task OnUpdateAsync(TMedia media, TMediaModel mediaModel)
        {
            NullGuard.NotNull(media, nameof(media))
                .NotNull(mediaModel, nameof(mediaModel));

            mediaModel.CoreData.Title = media.Title;
            mediaModel.CoreData.Subtitle = media.Subtitle;
            mediaModel.CoreData.Description = media.Description;
            mediaModel.CoreData.Details = media.Details;
            mediaModel.CoreData.MediaTypeIdentifier = media.MediaType.Number;
            mediaModel.CoreData.MediaType = await DbContext.MediaTypes.SingleAsync(m => m.MediaTypeIdentifier == media.MediaType.Number);
            mediaModel.CoreData.Published = media.Published;
            mediaModel.CoreData.Url = ValueConverter.UriToString(media.Url);
            mediaModel.CoreData.Image = ValueConverter.ByteArrayToString(media.Image);

            await HandleMediaBindingsAsync(media.GetMediaBindings().ToArray(), mediaModel, GetMediaBindingModels(mediaModel));

        }

		protected sealed override Task<bool> CanDeleteAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            return Task.FromResult(true);
        }

        protected override Task<TMediaModel> OnDeleteAsync(TMediaModel mediaModel)
        {
            NullGuard.NotNull(mediaModel, nameof(mediaModel));

            DbContext.MediaCoreData.Remove(mediaModel.CoreData);

            return Task.FromResult(mediaModel);
        }

        protected virtual IQueryable<TMediaModel> CreateReader()
        {
	        IQueryable<TMediaModel> reader = Entities;

	        return _includeCoreData == false ? reader : reader.Include(m => m.CoreData).ThenInclude(m => m.MediaType);
        }

        protected async Task<List<TMediaBindingModel>> HandleMediaBindingsAsync(IMediaBinding[] mediaBindings, TMediaModel mediaModel, List<TMediaBindingModel> mediaBindingModels)
        {
	        NullGuard.NotNull(mediaBindings, nameof(mediaBindings))
		        .NotNull(mediaModel, nameof(mediaModel))
		        .NotNull(mediaBindingModels, nameof(mediaBindingModels));

	        foreach (IMediaBinding mediaBinding in mediaBindings)
	        {
		        TMediaBindingModel mediaBindingModel = mediaBindingModels.FindMatchingMediaBindingModel(mediaBinding);
                if (mediaBindingModel != null)
                {
	                continue;
                }

                MediaPersonalityModel mediaPersonalityModel = await DbContext.MediaPersonalities.SingleAsync(m => m.ExternalMediaPersonalityIdentifier == ValueConverter.GuidToString(mediaBinding.MediaPersonality.MediaPersonalityIdentifier));

                TMediaBindingModel mediaBindingModelToCreate = BuildMediaBindingModel(mediaModel, mediaPersonalityModel, (short)mediaBinding.Role);
                mediaBindingModels.Add(mediaBindingModelToCreate);
	        }

	        TMediaBindingModel mediaBindingModelToDelete = mediaBindingModels.FirstOrDefault(mediaBindingModel => mediaBindings.Any(mediaBindingModel.IsMatchingMediaBindingModel) == false);
	        while (mediaBindingModelToDelete != null)
	        {
		        mediaBindingModels.Remove(mediaBindingModelToDelete);

                mediaBindingModelToDelete = mediaBindingModels.FirstOrDefault(mediaBindingModel => mediaBindings.Any(mediaBindingModel.IsMatchingMediaBindingModel) == false);
	        }

	        return mediaBindingModels;
        }

        protected abstract List<TMediaBindingModel> GetMediaBindingModels(TMediaModel mediaModel);

        protected abstract TMediaBindingModel BuildMediaBindingModel(TMediaModel mediaModel, MediaPersonalityModel mediaPersonalityModel, short role);

        #endregion
    }
}