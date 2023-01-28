using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
    internal class UpdateMovieGenreCommand : UpdateGenericCategoryCommandBase<IMovieGenre>, IUpdateMovieGenreCommand
    {
        #region Constructor

        public UpdateMovieGenreCommand(int number, string name) 
            : base(number, name)
        {
        }

        #endregion

        #region Methods

        public override IMovieGenre ToDomain()
        {
            return new MovieGenre(Number, Name);
        }

        #endregion
    }
}