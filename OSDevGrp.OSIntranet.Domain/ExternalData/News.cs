using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;

namespace OSDevGrp.OSIntranet.Domain.ExternalData
{
    public class News : INews
    {
        #region Constructors

        public News(string identifier, DateTimeOffset timestamp, string header, string details, string provider, string author, Uri sourceUrl, Uri imageUrl)
            : this(identifier, timestamp.LocalDateTime, header, details, provider, author, sourceUrl, imageUrl)
        {
        }

        public News(string identifer, DateTime timestamp, string header, string details, string provider, string author, Uri sourceUrl, Uri imageUrl)
        {
            NullGuard.NotNullOrWhiteSpace(identifer, nameof(identifer))
                .NotNullOrWhiteSpace(header, nameof(header));

            Identifier = identifer.Trim();
            Timestamp = timestamp;
            Header = header.Trim();
            Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
            Provider = string.IsNullOrWhiteSpace(provider) ? null : provider.Trim();
            Author = string.IsNullOrWhiteSpace(author) ? null : author.Trim();
            SourceUrl = sourceUrl;
            ImageUrl = imageUrl;
        }

        #endregion

        #region Properties

        public string Identifier { get; }

        public DateTime Timestamp { get; }

        public string Header { get; }

        public string Details { get; }

        public string Provider { get; }

        public string Author { get; }

        public Uri SourceUrl { get; }

        public Uri ImageUrl { get; }

        #endregion
    }  
}