using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class DeleteBookGenreCommand : DeleteGenericCategoryCommandBase<IBookGenre>, IDeleteBookGenreCommand
	{
		#region Constructor

		public DeleteBookGenreCommand(int number) 
			: base(number)
		{
		}

		#endregion
	}
}
