using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public class MovieGenre : GenericCategoryBase, IMovieGenre
    {
        #region Constructor

        public MovieGenre(int number, string name)
            : base(number, name)
        {
        }

        #endregion
    }
}