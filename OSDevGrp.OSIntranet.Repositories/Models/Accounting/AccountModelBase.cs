using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Converters.Extensions;
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

        public virtual DateTime StatusDate { get; set; }

        public virtual DateTime StatusDateForInfos => StatusDate.GetStatusDateForInfos();

        public virtual bool Deletable { get; set; }

        public virtual List<PostingLineModel> PostingLines { get; set; }

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

            AuditModelBase lastModifiedInfoModel = GetLastModifiedInfoModel();
            if (lastModifiedInfoModel != null && modifiedUtcDateTime < lastModifiedInfoModel.ModifiedUtcDateTime)
            {
                modifiedUtcDateTime = lastModifiedInfoModel.ModifiedUtcDateTime;
                modifiedByIdentifier = lastModifiedInfoModel.ModifiedByIdentifier;
            }

            auditable.AddAuditInformation(createdUtcDateTime, createdByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);
        }

        internal virtual DateTime GetFromDateForPostingLines() => DateTime.MinValue.Date;

        internal virtual DateTime GetToDateForPostingLines(int daysToAdd = 0) => StatusDate.AddDays(daysToAdd).Date;

        protected abstract AuditModelBase GetLastModifiedInfoModel();
    }
}