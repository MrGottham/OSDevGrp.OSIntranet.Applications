using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Core
{
    public interface IAuditable
    {
        DateTime CreatedDateTime { get; }

        string CreatedByIdentifier { get; }

        DateTime ModifiedDateTime { get; }

        string ModifiedByIdentifier { get; }

        void AddAuditInformation(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier);
    }
}