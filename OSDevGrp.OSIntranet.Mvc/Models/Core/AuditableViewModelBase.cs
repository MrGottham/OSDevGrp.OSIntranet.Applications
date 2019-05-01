using System;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Core
{
    public abstract class AuditableViewModelBase
    {
        #region Properties

        public EditMode EditMode { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Oprettet", ShortName = "Oprettet", Description = "Oprettet")]
        public string CreatedInfo => $"{CreatedDateTime.ToShortDateString()} kl. {CreatedDateTime.ToShortTimeString()} af {CreatedByIdentifier}";

        public string CreatedByIdentifier { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedByIdentifier { get; set; }

        [Display(Name = "Rettet", ShortName = "Rettet", Description = "Rettet")]
        public string ModifiedInfo => $"{ModifiedDateTime.ToShortDateString()} kl. {ModifiedDateTime.ToShortTimeString()} af {ModifiedByIdentifier}";

        #endregion
    }
}