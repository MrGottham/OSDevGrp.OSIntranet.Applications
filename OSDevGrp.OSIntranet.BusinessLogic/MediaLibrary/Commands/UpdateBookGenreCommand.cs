using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class UpdateBookGenreCommand : UpdateGenericCategoryCommandBase<IBookGenre>, IUpdateBookGenreCommand
	{
		#region Constructor

		public UpdateBookGenreCommand(int number, string name) 
			: base(number, name)
		{
		}

		#endregion

		#region Methods

		public override IBookGenre ToDomain()
		{
			return new BookGenre(Number, Name);
		}

		#endregion
	}
}