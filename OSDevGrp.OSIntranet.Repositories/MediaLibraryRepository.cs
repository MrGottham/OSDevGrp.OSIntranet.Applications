using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal class MediaLibraryRepository : DatabaseRepositoryBase<RepositoryContext>, IMediaLibraryRepository
    {
        #region Constructors

        public MediaLibraryRepository(RepositoryContext dbContext, IConfiguration configuration, ILoggerFactory loggerFactory) 
            : base(dbContext, configuration, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IMediaType>> GetMediaTypesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using MediaTypeModelHandler handler = new MediaTypeModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IMediaType> GetMediaTypeAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MediaTypeModelHandler handler = new MediaTypeModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateMediaTypeAsync(IMediaType mediaType)
        {
            NullGuard.NotNull(mediaType, nameof(mediaType));

            return ExecuteAsync(async () =>
                {
                    using MediaTypeModelHandler handler = new MediaTypeModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.CreateAsync(mediaType);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateMediaTypeAsync(IMediaType mediaType)
        {
            NullGuard.NotNull(mediaType, nameof(mediaType));

            return ExecuteAsync(async () =>
                {
                    using MediaTypeModelHandler handler = new MediaTypeModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.UpdateAsync(mediaType);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteMediaTypeAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MediaTypeModelHandler handler = new MediaTypeModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}