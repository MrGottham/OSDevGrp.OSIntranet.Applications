using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.Common
{
    public class Language : GenericCategoryBase, ILanguage
    {
        #region Constructor

        public Language(int number, string name)
            : base(number, name)
        {
        }

        #endregion
    }
}