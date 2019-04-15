using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Core
{
    public abstract class AuditableBase : IAuditable
    {
        #region Properties

        public DateTime CreatedDateTime { get; private set; }

        public string CreatedByIdentifier { get; private set; }

        public DateTime ModifiedDateTime { get; private set; }

        public string ModifiedByIdentifier { get; private set; }

        #endregion

        #region Methods

        public void AddAuditInformations(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(createdByIdentifier, nameof(createdByIdentifier))
                .NotNullOrWhiteSpace(modifiedByIdentifier, nameof(modifiedByIdentifier));

            CreatedDateTime = createdUtcDateTime.ToLocalTime();
            CreatedByIdentifier = createdByIdentifier;
            ModifiedDateTime = modifiedUtcDateTime.ToLocalTime();
            ModifiedByIdentifier = modifiedByIdentifier;
        }

        #endregion
    }
}