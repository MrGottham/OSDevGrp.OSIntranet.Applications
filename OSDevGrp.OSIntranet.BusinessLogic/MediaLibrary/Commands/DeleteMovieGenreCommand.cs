using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
    internal class DeleteMovieGenreCommand : DeleteGenericCategoryCommandBase<IMovieGenre>, IDeleteMovieGenreCommand
    {
        #region Constructor

        public DeleteMovieGenreCommand(int number) 
            : base(number)
        {
        }

        #endregion
    }
}