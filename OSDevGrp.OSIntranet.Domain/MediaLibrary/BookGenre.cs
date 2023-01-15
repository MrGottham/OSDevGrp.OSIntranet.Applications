using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public class BookGenre : GenericCategoryBase, IBookGenre
    {
        #region Constructor

        public BookGenre(int number, string name)
            : base(number, name)
        {
        }

        #endregion
    }
}
