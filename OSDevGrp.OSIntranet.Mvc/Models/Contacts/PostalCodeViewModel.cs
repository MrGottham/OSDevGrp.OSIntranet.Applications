using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class PostalCodeViewModel : AuditableViewModelBase
    {
        #region Properties

        public CountryViewModel Country { get; set; }

        public string Code { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public bool Deletable { get; set; }

        #endregion
    }
}
