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
	internal class MediaTypeModelHandler : GenericCategoryModelHandlerBase<IMediaType, MediaTypeModel>
    {
        #region Methods

        public MediaTypeModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<MediaTypeModel> Entities => DbContext.MediaTypes;

        #endregion

        #region Methods

        protected override Expression<Func<MediaTypeModel, bool>> EntitySelector(int primaryKey) => mediaTypeModel => mediaTypeModel.MediaTypeIdentifier == primaryKey;

        protected override async Task<bool> CanDeleteAsync(MediaTypeModel mediaTypeModel)
        {
	        NullGuard.NotNull(mediaTypeModel, nameof(mediaTypeModel));

	        if (mediaTypeModel.CoreData != null)
	        {
		        return mediaTypeModel.CoreData.Any() == false;
	        }

	        return await DbContext.MediaCoreData.FirstOrDefaultAsync(mediaCoreDataModel => mediaCoreDataModel.MediaTypeIdentifier == mediaTypeModel.MediaTypeIdentifier) == null;
        }

		#endregion
	}
}