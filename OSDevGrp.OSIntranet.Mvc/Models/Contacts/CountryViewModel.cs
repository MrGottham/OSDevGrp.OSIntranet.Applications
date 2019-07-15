using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class CountryViewModel : AuditableViewModelBase
    {
        #region Properties

        public string Code { get; set; }

        public string Name { get; set; }

        public string UniversalName { get; set; }

        public string PhonePrefix { get; set; }

        #endregion
    }
}
