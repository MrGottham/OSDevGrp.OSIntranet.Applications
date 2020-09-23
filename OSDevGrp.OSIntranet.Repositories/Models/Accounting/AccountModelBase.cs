using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal abstract class AccountModelBase : AuditModelBase
    {
        public virtual int AccountingIdentifier { get; set; }

        public virtual AccountingModel Accounting { get; set; }

        public virtual string AccountNumber { get; set; }

        public virtual int BasicAccountIdentifier { get; set; }

        public virtual BasicAccountModel BasicAccount { get; set; }

        public virtual bool Deletable { get; set; }

        internal void CopyAuditInformationTo(IAuditable auditable)
        {
            NullGuard.NotNull(auditable, nameof(auditable));

            DateTime createdUtcDateTime = CreatedUtcDateTime;
            string createdByIdentifier = CreatedByIdentifier;
            if (createdUtcDateTime > BasicAccount.CreatedUtcDateTime)
            {
                createdUtcDateTime = BasicAccount.CreatedUtcDateTime;
                createdByIdentifier = BasicAccount.CreatedByIdentifier;
            }

            DateTime modifiedUtcDateTime = ModifiedUtcDateTime;
            string modifiedByIdentifier = ModifiedByIdentifier;
            if (modifiedUtcDateTime < BasicAccount.ModifiedUtcDateTime)
            {
                modifiedUtcDateTime = BasicAccount.ModifiedUtcDateTime;
                modifiedByIdentifier = BasicAccount.ModifiedByIdentifier;
            }

            auditable.AddAuditInformation(createdUtcDateTime, createdByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);
        }
    }
}