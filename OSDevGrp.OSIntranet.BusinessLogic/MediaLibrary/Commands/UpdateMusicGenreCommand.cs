using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
    internal class UpdateMusicGenreCommand : UpdateGenericCategoryCommandBase<IMusicGenre>, IUpdateMusicGenreCommand
    {
        #region Constuctor

        public UpdateMusicGenreCommand(int number, string name)
            : base(number, name)
        {
        }

        #endregion

        #region Methods

        public override IMusicGenre ToDomain()
        {
            return new MusicGenre(Number, Name);
        }

        #endregion
    }
}