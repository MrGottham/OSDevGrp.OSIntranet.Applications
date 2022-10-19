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

        public virtual void AddAuditInformation(DateTime createdUtcDateTime, string createdByIdentifier, DateTime modifiedUtcDateTime, string modifiedByIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(createdByIdentifier, nameof(createdByIdentifier))
                .NotNullOrWhiteSpace(modifiedByIdentifier, nameof(modifiedByIdentifier));

            SetAuditInformation(createdUtcDateTime.ToLocalTime(), createdByIdentifier, modifiedUtcDateTime.ToLocalTime(), modifiedByIdentifier);
        }

        protected void SetAuditInformation(DateTime createdDateTime, string createdByIdentifier, DateTime modifiedDateTime, string modifiedByIdentifier)
        {
            NullGuard.NotNullOrWhiteSpace(createdByIdentifier, nameof(createdByIdentifier))
                .NotNullOrWhiteSpace(modifiedByIdentifier, nameof(modifiedByIdentifier));

            CreatedDateTime = createdDateTime;
            CreatedByIdentifier = createdByIdentifier;
            ModifiedDateTime = modifiedDateTime;
            ModifiedByIdentifier = modifiedByIdentifier;
        }

        #endregion
    }
}