using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	public class MediaLibraryRepositoryTestBase : DatabaseRepositoryTestBase
    {
	    #region Private variables

	    private static Guid? _existingMediaPersonalityIdentifier;

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

        #endregion
	}
}