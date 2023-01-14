using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
    public class MediaType : GenericCategoryBase, IMediaType
    {
        #region Constructor

        public MediaType(int number, string name) 
            : base(number, name)
        {
        }

        #endregion
    }
}