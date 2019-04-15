using System;

namespace OSDevGrp.OSIntranet.Mvc.Models.Core
{
    public abstract class AuditableViewModelBase
    {
        #region Properties

        public DateTime CreatedDateTime { get; set; }

        public string CreatedByIdentifier { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedByIdentifier { get; set; }

        #endregion
    }
}