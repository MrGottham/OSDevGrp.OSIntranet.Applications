using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
    public static class MediaLibraryCommandFactory
    {
        #region Methods

        public static ICreateMovieGenreCommand BuildCreateMovieGenreCommand(int number, string name)
        {
            return new CreateMovieGenreCommand(number, name);
        }

        public static IUpdateMovieGenreCommand BuildUpdateMovieGenreCommand(int number, string name)
        {
            return new UpdateMovieGenreCommand(number, name);
        }

        public static IDeleteMovieGenreCommand BuildDeleteMovieGenreCommand(int number)
        {
            return new DeleteMovieGenreCommand(number);
        }

        public static ICreateMusicGenreCommand BuildCreateMusicGenreCommand(int number, string name)
        {
	        return new CreateMusicGenreCommand(number, name);
        }

        public static IUpdateMusicGenreCommand BuildUpdateMusicGenreCommand(int number, string name)
        {
	        return new UpdateMusicGenreCommand(number, name);
        }

        public static IDeleteMusicGenreCommand BuildDeleteMusicGenreCommand(int number)
        {
	        return new DeleteMusicGenreCommand(number);
        }

        public static ICreateBookGenreCommand BuildCreateBookGenreCommand(int number, string name)
        {
	        return new CreateBookGenreCommand(number, name);
        }

        public static IUpdateBookGenreCommand BuildUpdateBookGenreCommand(int number, string name)
        {
	        return new UpdateBookGenreCommand(number, name);
        }

        public static IDeleteBookGenreCommand BuildDeleteBookGenreCommand(int number)
        {
	        return new DeleteBookGenreCommand(number);
        }

        public static ICreateMediaTypeCommand BuildCreateMediaTypeCommand(int number, string name)
        {
	        return new CreateMediaTypeCommand(number, name);
        }

        public static IUpdateMediaTypeCommand BuildUpdateMediaTypeCommand(int number, string name)
        {
	        return new UpdateMediaTypeCommand(number, name);
        }

        public static IDeleteMediaTypeCommand BuildDeleteMediaTypeCommand(int number)
        {
	        return new DeleteMediaTypeCommand(number);
        }

        #endregion
	}
}