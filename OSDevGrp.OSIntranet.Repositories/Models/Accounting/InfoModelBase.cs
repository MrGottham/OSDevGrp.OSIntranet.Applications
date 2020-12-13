using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal abstract class InfoModelBase : AuditModelBase
    {
        public virtual int YearMonthIdentifier { get; set; }

        public virtual YearMonthModel YearMonth { get; set; }

        public virtual bool Deletable { get; set; }

        internal void CopyAuditInformationTo(IAuditable auditable)
        {
            NullGuard.NotNull(auditable, nameof(auditable));

            DateTime createdUtcDateTime = CreatedUtcDateTime;
            string createdByIdentifier = CreatedByIdentifier;
            if (createdUtcDateTime > YearMonth.CreatedUtcDateTime)
            {
                createdUtcDateTime = YearMonth.CreatedUtcDateTime;
                createdByIdentifier = YearMonth.CreatedByIdentifier;
            }

            DateTime modifiedUtcDateTime = ModifiedUtcDateTime;
            string modifiedByIdentifier = ModifiedByIdentifier;
            if (modifiedUtcDateTime < YearMonth.ModifiedUtcDateTime)
            {
                modifiedUtcDateTime = YearMonth.ModifiedUtcDateTime;
                modifiedByIdentifier = YearMonth.ModifiedByIdentifier;
            }

            auditable.AddAuditInformation(createdUtcDateTime, createdByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);
        }
    }
}