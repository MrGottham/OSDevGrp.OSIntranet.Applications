using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Common
{
    public class Nationality : GenericCategoryBase, INationality
    {
        #region Constructor

        public Nationality(int number, string name) 
            : base(number, name)
        {
        }

        #endregion
    }
}