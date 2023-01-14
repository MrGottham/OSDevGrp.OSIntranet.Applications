using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
    public interface IMedia : IAuditable, IDeletable
    {
        Guid MediaIdentifier { get; }

        string Title { get; }

        string Subtitle { get; }

        string Description { get; }

        IMediaType MediaType { get; }

        short? Published { get; }

        string Details { get; }

        byte[] Image { get; }
    }
}