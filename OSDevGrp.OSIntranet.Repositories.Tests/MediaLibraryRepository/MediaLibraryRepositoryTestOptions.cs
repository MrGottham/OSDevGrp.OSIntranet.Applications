using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    internal class MediaLibraryRepositoryTestOptions
    {
        public Guid ExistingMediaPersonalityIdentifier { get; set; }

        public Guid ExistingMovieIdentifier { get; set; }

        public Guid ExistingMusicIdentifier { get; set; }

        public Guid ExistingBookIdentifier { get; set; }

        public Guid? ExistingBorrowerIdentifier { get; set; }

        public Guid? ExistingLendingIdentifier { get; set; }
    }
}