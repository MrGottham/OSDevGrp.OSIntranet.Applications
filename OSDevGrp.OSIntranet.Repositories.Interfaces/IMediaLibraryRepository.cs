using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
	public interface IMediaLibraryRepository : IRepository
	{
		Task<IEnumerable<IMediaPersonality>> GetMediaPersonalitiesAsync();

		Task<bool> MediaPersonalityExistsAsync(Guid mediaPersonalityIdentifier);

		Task<IMediaPersonality> GetMediaPersonalityAsync(Guid mediaPersonalityIdentifier);

		Task CreateMediaPersonalityAsync(IMediaPersonality mediaPersonality);

		Task UpdateMediaPersonalityAsync(IMediaPersonality mediaPersonality);

		Task DeleteMediaPersonalityAsync(Guid mediaPersonalityIdentifier);

		Task<IEnumerable<IMovieGenre>> GetMovieGenresAsync();

        Task<IMovieGenre> GetMovieGenreAsync(int number);

        Task CreateMovieGenreAsync(IMovieGenre movieGenre);

        Task UpdateMovieGenreAsync(IMovieGenre movieGenre);

        Task DeleteMovieGenreAsync(int number);

        Task<IEnumerable<IMusicGenre>> GetMusicGenresAsync();

        Task<IMusicGenre> GetMusicGenreAsync(int number);

        Task CreateMusicGenreAsync(IMusicGenre musicGenre);

        Task UpdateMusicGenreAsync(IMusicGenre musicGenre);

        Task DeleteMusicGenreAsync(int number);

        Task<IEnumerable<IBookGenre>> GetBookGenresAsync();

        Task<IBookGenre> GetBookGenreAsync(int number);

        Task CreateBookGenreAsync(IBookGenre bookGenre);

        Task UpdateBookGenreAsync(IBookGenre bookGenre);

        Task DeleteBookGenreAsync(int number);

        Task<IEnumerable<IMediaType>> GetMediaTypesAsync();

        Task<IMediaType> GetMediaTypeAsync(int number);

        Task CreateMediaTypeAsync(IMediaType mediaType);

        Task UpdateMediaTypeAsync(IMediaType mediaType);

        Task DeleteMediaTypeAsync(int number);
    }
}