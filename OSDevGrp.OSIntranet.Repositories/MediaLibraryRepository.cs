﻿using Microsoft.Extensions.Logging;
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
    internal class MediaLibraryRepository(RepositoryContext dbContext, ILoggerFactory loggerFactory) : DatabaseRepositoryBase<RepositoryContext>(dbContext, loggerFactory), IMediaLibraryRepository
    {
		#region Methods

		public Task<IEnumerable<IMedia>> GetMediasAsync(string titleFilter = null)
		{
			return ExecuteAsync<IEnumerable<IMedia>>(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					using MovieModelHandler movieModelHandler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
					using MusicModelHandler musicModelHandler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
					using BookModelHandler bookModelHandler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);

					List<IMedia> mediaCollection = new List<IMedia>();
					if (string.IsNullOrWhiteSpace(titleFilter))
					{
						mediaCollection.AddRange(await movieModelHandler.ReadAsync());
						mediaCollection.AddRange(await musicModelHandler.ReadAsync());
						mediaCollection.AddRange(await bookModelHandler.ReadAsync());
					}
					else
					{
						mediaCollection.AddRange(await movieModelHandler.ReadAsync(movieModel => movieModel.CoreData.Title.Contains(titleFilter) || (movieModel.CoreData.Subtitle != null && movieModel.CoreData.Subtitle.Contains(titleFilter))));
						mediaCollection.AddRange(await musicModelHandler.ReadAsync(musicModel => musicModel.CoreData.Title.Contains(titleFilter) || (musicModel.CoreData.Subtitle != null && musicModel.CoreData.Subtitle.Contains(titleFilter))));
						mediaCollection.AddRange(await bookModelHandler.ReadAsync(bookModel => bookModel.CoreData.Title.Contains(titleFilter) || (bookModel.CoreData.Subtitle != null && bookModel.CoreData.Subtitle.Contains(titleFilter))));
					}

					HashSet<IMedia> mediaHashSet = new HashSet<IMedia>(mediaCollection);

					return mediaHashSet.OrderBy(media => media.ToString()).ToArray();
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<IEnumerable<TMedia>> GetMediasAsync<TMedia>(string titleFilter = null) where TMedia : class, IMedia
		{
			return ExecuteAsync<IEnumerable<TMedia>>(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);

						IEnumerable<IMovie> movies = string.IsNullOrWhiteSpace(titleFilter)
							? await handler.ReadAsync()
							: await handler.ReadAsync(movieModel => movieModel.CoreData.Title.Contains(titleFilter) || (movieModel.CoreData.Subtitle != null && movieModel.CoreData.Subtitle.Contains(titleFilter)));

						return movies.OfType<TMedia>().ToArray();
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);

						IEnumerable<IMusic> music = string.IsNullOrWhiteSpace(titleFilter)
							? await handler.ReadAsync()
							: await handler.ReadAsync(musicModel => musicModel.CoreData.Title.Contains(titleFilter) || (musicModel.CoreData.Subtitle != null && musicModel.CoreData.Subtitle.Contains(titleFilter)));

						return music.OfType<TMedia>().ToArray();
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);

						IEnumerable<IBook> books = string.IsNullOrWhiteSpace(titleFilter)
							? await handler.ReadAsync()
							: await handler.ReadAsync(bookModel => bookModel.CoreData.Title.Contains(titleFilter) || (bookModel.CoreData.Subtitle != null && bookModel.CoreData.Subtitle.Contains(titleFilter)));

						return books.OfType<TMedia>().ToArray();
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
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return await handler.ReadAsync(mediaIdentifier) != null;
					}

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<bool> MediaExistsAsync<TMedia>(string title, string subtitle, int mediaTypeIdentifier) where TMedia : class, IMedia
		{
			NullGuard.NotNullOrWhiteSpace(title, nameof(title));

			return ExecuteAsync(async () =>
				{
					IConverter mediaLibraryModelConverter = MediaLibraryModelConverter.Create();

					if (typeof(TMedia) == typeof(IMovie))
					{
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return (await handler.ReadAsync(movieModel => movieModel.CoreData.Title == title && movieModel.CoreData.Subtitle == subtitle && movieModel.CoreData.MediaTypeIdentifier == mediaTypeIdentifier)).Any();
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return (await handler.ReadAsync(musicModel => musicModel.CoreData.Title == title && musicModel.CoreData.Subtitle == subtitle && musicModel.CoreData.MediaTypeIdentifier == mediaTypeIdentifier)).Any();
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, false, false);
						return (await handler.ReadAsync(bookModel => bookModel.CoreData.Title == title && bookModel.CoreData.Subtitle == subtitle && bookModel.CoreData.MediaTypeIdentifier == mediaTypeIdentifier)).Any();
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
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
						return await handler.ReadAsync(mediaIdentifier) as TMedia;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
						return await handler.ReadAsync(mediaIdentifier) as TMedia;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
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
						using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
						await handler.CreateAsync(media as IMovie);
						return;
					}

					if (typeof(TMedia) == typeof(IMusic))
					{
						using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
						await handler.CreateAsync(media as IMusic);
						return;
					}

					if (typeof(TMedia) == typeof(IBook))
					{
						using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
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
				        using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
				        await handler.UpdateAsync(media as IMovie);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IMusic))
			        {
				        using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
				        await handler.UpdateAsync(media as IMusic);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IBook))
			        {
				        using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
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
				        using MovieModelHandler handler = new MovieModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

			        if (typeof(TMedia) == typeof(IMusic))
			        {
				        using MusicModelHandler handler = new MusicModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

					if (typeof(TMedia) == typeof(IBook))
			        {
				        using BookModelHandler handler = new BookModelHandler(DbContext, mediaLibraryModelConverter, true, true, true);
				        await handler.DeleteAsync(mediaIdentifier);
				        return;
			        }

					throw new NotSupportedException($"{typeof(TMedia)} is not supported as {nameof(TMedia)}.");
		        },
		        MethodBase.GetCurrentMethod());
        }

		public Task<IEnumerable<IMediaPersonality>> GetMediaPersonalitiesAsync(string nameFilter = null)
        {
	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
			        return string.IsNullOrWhiteSpace(nameFilter)
				        ? await handler.ReadAsync()
				        : await handler.ReadAsync(mediaPersonalityModel => (mediaPersonalityModel.GivenName != null && mediaPersonalityModel.GivenName.Contains(nameFilter)) || (mediaPersonalityModel.MiddleName != null && mediaPersonalityModel.MiddleName.Contains(nameFilter)) || mediaPersonalityModel.Surname.Contains(nameFilter));
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

        public Task<bool> MediaPersonalityExistsAsync(string givenName, string middleName, string surname, DateTime? birthDate)
        {
	        NullGuard.NotNullOrWhiteSpace(surname, nameof(surname));

	        return ExecuteAsync(async () =>
		        {
			        using MediaPersonalityModelHandler handler = new MediaPersonalityModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
			        return (await handler.ReadAsync(mediaPersonalityModel => mediaPersonalityModel.GivenName == givenName && mediaPersonalityModel.MiddleName == middleName && mediaPersonalityModel.Surname == surname && mediaPersonalityModel.BirthDate == birthDate)).Any();
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

		public Task<IEnumerable<IBorrower>> GetBorrowersAsync(string fullNameFilter = null)
		{
			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
					return string.IsNullOrWhiteSpace(fullNameFilter)
						? await handler.ReadAsync()
						: await handler.ReadAsync(borrowerModel => borrowerModel.FullName.Contains(fullNameFilter));
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<bool> BorrowerExistsAsync(Guid borrowerIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
					return await handler.ReadAsync(borrowerIdentifier) != null;
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<bool> BorrowerExistsAsync(string fullName)
		{
			NullGuard.NotNullOrWhiteSpace(fullName, nameof(fullName));

			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
					return (await handler.ReadAsync(borrowerModel => borrowerModel.FullName == fullName)).Any();
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<IBorrower> GetBorrowerAsync(Guid borrowerIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
					return await handler.ReadAsync(borrowerIdentifier);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task CreateBorrowerAsync(IBorrower borrower)
		{
			NullGuard.NotNull(borrower, nameof(borrower));

			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), false);
					await handler.CreateAsync(borrower);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task UpdateBorrowerAsync(IBorrower borrower)
		{
			NullGuard.NotNull(borrower, nameof(borrower));

			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
					await handler.UpdateAsync(borrower);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task DeleteBorrowerAsync(Guid borrowerIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using BorrowerModelHandler handler = new BorrowerModelHandler(DbContext, MediaLibraryModelConverter.Create(), true);
					await handler.DeleteAsync(borrowerIdentifier);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<IEnumerable<ILending>> GetLendingsAsync(bool includeReturned = true)
		{
			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					return includeReturned
						? await handler.ReadAsync()
						: await handler.ReadAsync(lendingModel => lendingModel.ReturnedDate == null || (lendingModel.ReturnedDate != null && lendingModel.ReturnedDate > DateTime.Today));
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<bool> LendingExistsAsync(Guid lendingIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					return await handler.ReadAsync(lendingIdentifier) != null;
				},
				MethodBase.GetCurrentMethod());
		}

		public Task<ILending> GetLendingAsync(Guid lendingIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					return await handler.ReadAsync(lendingIdentifier);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task CreateLendingAsync(ILending lending)
		{
			NullGuard.NotNull(lending, nameof(lending));

			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					await handler.CreateAsync(lending);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task UpdateLendingAsync(ILending lending)
		{
			NullGuard.NotNull(lending, nameof(lending));

			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					await handler.UpdateAsync(lending);
				},
				MethodBase.GetCurrentMethod());
		}

		public Task DeleteLendingAsync(Guid lendingIdentifier)
		{
			return ExecuteAsync(async () =>
				{
					using LendingModelHandler handler = new LendingModelHandler(DbContext, MediaLibraryModelConverter.Create(), true, true);
					await handler.DeleteAsync(lendingIdentifier);
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