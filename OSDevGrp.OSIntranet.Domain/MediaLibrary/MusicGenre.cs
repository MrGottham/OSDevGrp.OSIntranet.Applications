using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public class MusicGenre : GenericCategoryBase, IMusicGenre
    {
        #region Constructor

        public MusicGenre(int number, string name)
            : base(number, name)
        {
        }

        #endregion
    }
}