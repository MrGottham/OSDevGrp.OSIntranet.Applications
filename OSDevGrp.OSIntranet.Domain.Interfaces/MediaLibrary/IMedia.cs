using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
    public interface IMedia : IAuditable, IDeletable
    {
        Guid MediaIdentifier { get; }

        string Name { get; }

        string Description { get; }

        byte[] Image { get; }
    }
}