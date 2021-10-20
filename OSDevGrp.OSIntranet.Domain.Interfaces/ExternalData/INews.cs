using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData
{
    public interface INews
    {
        string Identifier { get; }

        DateTime Timestamp { get; }

        string Header { get; }

        string Details { get; }

        string Provider { get; }

        string Author { get; }

        Uri SourceUrl { get; }

        Uri ImageUrl { get; }
    }
}