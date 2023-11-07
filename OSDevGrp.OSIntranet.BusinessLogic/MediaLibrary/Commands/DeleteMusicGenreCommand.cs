using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class DeleteMusicGenreCommand : DeleteGenericCategoryCommandBase<IMusicGenre>, IDeleteMusicGenreCommand
	{
		#region Constructor

		public DeleteMusicGenreCommand(int number) 
			: base(number)
		{
		}

		#endregion
	}
}