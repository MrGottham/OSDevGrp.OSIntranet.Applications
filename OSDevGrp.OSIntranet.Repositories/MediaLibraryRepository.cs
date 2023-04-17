using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public Task<IEnumerable<IMedia>> GetMediasAsync()
		{
			return ExecuteAsync<IEnumerable<IMedia>>(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					using MovieModelHandler movieModelHandler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
					using MusicModelHandler musicModelHandler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
					using BookModelHandler bookModelHandler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);

					List<IMedia> mediaCollection = new List<IMedia>();
					mediaCollection.AddRange(await movieModelHandler.ReadAsync());
					mediaCollection.AddRange(await musicModelHandler.ReadAsync());
					mediaCollection.AddRange(await bookModelHandler.ReadAsync());

                    HashSet<IMedia> mediaHashSet = new HashSet<IMedia>(mediaCollection);

					return mediaHashSet.OrderBy(media => media.ToString()).ToArray();
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<IEnumerable<TMedia>> GetMediasAsync<TMedia>() where TMedia : class, IMedia
		{
			return ExecuteAsync(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return (await handler.ReadAsync()).OfType<TMedia>();
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return (await handler.ReadAsync()).OfType<TMedia>();
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return (await handler.ReadAsync()).OfType<TMedia>();
					}

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<bool> MediaExistsAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia
		{
			return ExecuteAsync(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<TMedia> GetMediaAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia
		{
			return ExecuteAsync(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return await handler.ReadAsync(mediaIdentifier) as TMedia;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return await handler.ReadAsync(mediaIdentifier) as TMedia;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						return await handler.ReadAsync(mediaIdentifier) as TMedia;
					}

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
				},
				MethodBase.GetCurrentMethod());
		}

		public Task CreateMediaAsync<TMedia>(TMedia media) where TMedia : class, IMedia
		{
			NullGuard.NotNull(media, nameof(media));

			return ExecuteAsync(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						await handler.CreateAsync(media as IMovie);
						return;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						await handler.CreateAsync(media as IMusic);
						return;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);
						await handler.CreateAsync(media as IBook);
						return;
					}

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
				},
				MethodBase.GetCurrentMethod());
		}

        public Task UpdateMediaAsync<TMedia>(TMedia media) where TMedia : class, IMedia
        {
	        NullGuard.NotNull(media, nameof(media));

	        return ExecuteAsync(async () =>
		        {
			        IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

			        if (typeof(TMedia) == typeof(IMovie))
			        {
				        using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.UpdateAsync(media as IMovie);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IMusic))
			        {
				        using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.UpdateAsync(media as IMusic);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IBook))
			        {
				        using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.UpdateAsync(media as IBook);
				        return;
			        }

			        throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
		        },
		        MethodBase.GetCurrentMethod());
        }

        public Task DeleteMediaAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia
		{
	        return ExecuteAsync(async () =>
		        {
			        IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

			        if (typeof(TMedia) == typeof(IMovie))
			        {
				        using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IMusic))
			        {
				        using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

					if (typeof(TMedia) == typeof(IBook))
			        {
				        using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
		        },
		        MethodBase.GetCurrentMethod());
        }

		public Task<IEnumerable<IMediaPersonality>> GetMediaPersonalitiesAsync()
        {
	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
			        return await handler.ReadAsync();
		        },
		        MethodBase.GetCurrentMethod());
        }

        public Task<bool> MediaPersonalityExistsAsync(Guid mediaPersonalityIdentifier)
        {
	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
			        return await handler.ReadAsync(mediaPersonalityIdentifier) != null;
		        },
		        MethodBase.GetCurrentMethod());
        }

        public Task<IMediaPersonality> GetMediaPersonalityAsync(Guid mediaPersonalityIdentifier)
        {
	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
			        return await handler.ReadAsync(mediaPersonalityIdentifier);
		        },
		        MethodBase.GetCurrentMethod());
        }

        public Task CreateMediaPersonalityAsync(IMediaPersonality mediaPersonality)
        {
	        NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality));

	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
			        await handler.CreateAsync(mediaPersonality);
		        },
		        MethodBase.GetCurrentMethod());
        }

        public Task UpdateMediaPersonalityAsync(IMediaPersonality mediaPersonality)
        {
	        NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality));

	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
			        await handler.UpdateAsync(mediaPersonality);
		        },
		        MethodBase.GetCurrentMethod());
        }

		public Task DeleteMediaPersonalityAsync(Guid mediaPersonalityIdentifier)
        {
	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
			        await handler.DeleteAsync(mediaPersonalityIdentifier);
		        },
		        MethodBase.GetCurrentMethod());
        }

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