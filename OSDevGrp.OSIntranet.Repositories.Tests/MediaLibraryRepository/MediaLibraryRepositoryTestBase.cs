using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	public class MediaLibraryRepositoryTestBase : DatabaseRepositoryTestBase
    {
	    #region Private variables

	    private static Guid? _existingMediaPersonalityIdentifier;
	    private static Guid? _existingMovieIdentifier;
	    private static Guid? _existingMusicIdentifier;
	    private static Guid? _existingBookIdentifier;

		#endregion

		#region Methods

		protected IMediaLibraryRepository CreateSut()
        {
            return new Repositories.MediaLibraryRepository(CreateTestRepositoryContext(), CreateTestConfiguration(), CreateLoggerFactory());
        }

		protected Guid WithExistingMediaPersonalityIdentifier()
		{
			lock (SyncRoot)
			{
				if (_existingMediaPersonalityIdentifier.HasValue)
				{
					return _existingMediaPersonalityIdentifier.Value;
				}

				IConfiguration configuration = CreateTestConfiguration();
				// ReSharper disable AssignNullToNotNullAttribute
				return (_existingMediaPersonalityIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMediaPersonalityIdentifier"])).Value;
				// ReSharper restore AssignNullToNotNullAttribute
			}
		}

		protected Guid WithExistingMovieIdentifier()
		{
			lock (SyncRoot)
			{
				if (_existingMovieIdentifier.HasValue)
				{
					return _existingMovieIdentifier.Value;
				}

				IConfiguration configuration = CreateTestConfiguration();
				// ReSharper disable AssignNullToNotNullAttribute
				return (_existingMovieIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMovieIdentifier"])).Value;
				// ReSharper restore AssignNullToNotNullAttribute
			}
		}

		protected Guid WithExistingMusicIdentifier()
		{
			lock (SyncRoot)
			{
				if (_existingMusicIdentifier.HasValue)
				{
					return _existingMusicIdentifier.Value;
				}

				IConfiguration configuration = CreateTestConfiguration();
				// ReSharper disable AssignNullToNotNullAttribute
				return (_existingMusicIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingMusicIdentifier"])).Value;
				// ReSharper restore AssignNullToNotNullAttribute
			}
		}

		protected Guid WithExistingBookIdentifier()
		{
			lock (SyncRoot)
			{
				if (_existingBookIdentifier.HasValue)
				{
					return _existingBookIdentifier.Value;
				}

				IConfiguration configuration = CreateTestConfiguration();
				// ReSharper disable AssignNullToNotNullAttribute
				return (_existingBookIdentifier = Guid.Parse(configuration["TestData:MediaLibrary:ExistingBookIdentifier"])).Value;
				// ReSharper restore AssignNullToNotNullAttribute
			}
		}

        #endregion
	}
}