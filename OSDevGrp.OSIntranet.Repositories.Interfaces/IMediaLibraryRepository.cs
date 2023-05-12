using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
	public interface IMediaLibraryRepository : IRepository
	{
		Task<IEnumerable<IMedia>> GetMediasAsync(string titleFilter = null);

		Task<IEnumerable<TMedia>> GetMediasAsync<TMedia>(string titleFilter = null) where TMedia : class, IMedia;

		Task<bool> MediaExistsAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia;

		Task<bool> MediaExistsAsync<TMedia>(string title, string subtitle) where TMedia : class, IMedia;

		Task<TMedia> GetMediaAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia;

		Task CreateMediaAsync<TMedia>(TMedia media) where TMedia : class, IMedia;

		Task UpdateMediaAsync<TMedia>(TMedia media) where TMedia : class, IMedia;

		Task DeleteMediaAsync<TMedia>(Guid mediaIdentifier) where TMedia : class, IMedia;

        Task<IEnumerable<IMediaPersonality>> GetMediaPersonalitiesAsync(string nameFilter = null);

		Task<bool> MediaPersonalityExistsAsync(Guid mediaPersonalityIdentifier);

		Task<bool> MediaPersonalityExistsAsync(string givenName, string middleName, string fullName);

		Task<IMediaPersonality> GetMediaPersonalityAsync(Guid mediaPersonalityIdentifier);

		Task CreateMediaPersonalityAsync(IMediaPersonality mediaPersonality);

		Task UpdateMediaPersonalityAsync(IMediaPersonality mediaPersonality);

		Task DeleteMediaPersonalityAsync(Guid mediaPersonalityIdentifier);

		Task<IEnumerable<IBorrower>> GetBorrowersAsync(string fullNameFilter = null);

		Task<bool> BorrowerExistsAsync(Guid borrowerIdentifier);

		Task<bool> BorrowerExistsAsync(string fullName);

		Task<IBorrower> GetBorrowerAsync(Guid borrowerIdentifier);

		Task CreateBorrowerAsync(IBorrower borrower);

		Task UpdateBorrowerAsync(IBorrower borrower);

		Task DeleteBorrowerAsync(Guid borrowerIdentifier);

		Task<IEnumerable<ILending>> GetLendingsAsync(bool includeReturned = true);

		Task<bool> LendingExistsAsync(Guid lendingIdentifier);

		Task<ILending> GetLendingAsync(Guid lendingIdentifier);

		Task CreateLendingAsync(ILending lending);

		Task UpdateLendingAsync(ILending lending);

		Task DeleteLendingAsync(Guid lendingIdentifier);

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