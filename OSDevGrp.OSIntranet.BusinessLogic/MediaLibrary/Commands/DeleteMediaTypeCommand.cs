using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class DeleteMediaTypeCommand : DeleteGenericCategoryCommandBase<IMediaType>, IDeleteMediaTypeCommand
	{
		#region Constructor

		public DeleteMediaTypeCommand(int number)
			: base(number)
		{
		}

		#endregion
	}
}