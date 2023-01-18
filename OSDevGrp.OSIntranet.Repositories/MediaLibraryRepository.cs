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

        public Task<IEnumerable<IMovieGenre>> GetMovieGenresAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using MovieGenreModelHandler handler = new MovieGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IMovieGenre> GetMovieGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MovieGenreModelHandler handler = new MovieGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateMovieGenreAsync(IMovieGenre movieGenre)
        {
            NullGuard.NotNull(movieGenre, nameof(movieGenre));

            return ExecuteAsync(async () =>
                {
                    using MovieGenreModelHandler handler = new MovieGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.CreateAsync(movieGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateMovieGenreAsync(IMovieGenre movieGenre)
        {
            NullGuard.NotNull(movieGenre, nameof(movieGenre));

            return ExecuteAsync(async () =>
                {
                    using MovieGenreModelHandler handler = new MovieGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.UpdateAsync(movieGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteMovieGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MovieGenreModelHandler handler = new MovieGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IMusicGenre>> GetMusicGenresAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using MusicGenreModelHandler handler = new MusicGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IMusicGenre> GetMusicGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MusicGenreModelHandler handler = new MusicGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateMusicGenreAsync(IMusicGenre musicGenre)
        {
            NullGuard.NotNull(musicGenre, nameof(musicGenre));

            return ExecuteAsync(async () =>
                {
                    using MusicGenreModelHandler handler = new MusicGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.CreateAsync(musicGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateMusicGenreAsync(IMusicGenre musicGenre)
        {
            NullGuard.NotNull(musicGenre, nameof(musicGenre));

            return ExecuteAsync(async () =>
                {
                    using MusicGenreModelHandler handler = new MusicGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.UpdateAsync(musicGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteMusicGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using MusicGenreModelHandler handler = new MusicGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IBookGenre>> GetBookGenresAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using BookGenreModelHandler handler = new BookGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBookGenre> GetBookGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BookGenreModelHandler handler = new BookGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    return await handler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task CreateBookGenreAsync(IBookGenre bookGenre)
        {
            NullGuard.NotNull(bookGenre, nameof(bookGenre));

            return ExecuteAsync(async () =>
                {
                    using BookGenreModelHandler handler = new BookGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.CreateAsync(bookGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task UpdateBookGenreAsync(IBookGenre bookGenre)
        {
            NullGuard.NotNull(bookGenre, nameof(bookGenre));

            return ExecuteAsync(async () =>
                {
                    using BookGenreModelHandler handler = new BookGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.UpdateAsync(bookGenre);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task DeleteBookGenreAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BookGenreModelHandler handler = new BookGenreModelHandler(DbContext, MediaLibraryModelConverter.Create());
                    await handler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

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