using OSDevGrp.OSIntranet.BusinessLogic.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal class CreateMediaTypeCommand : CreateGenericCategoryCommandBase<IMediaType>, ICreateMediaTypeCommand
	{
		#region Constructor

		public CreateMediaTypeCommand(int number, string name) 
			: base(number, name)
		{
		}

		#endregion

		#region Methods

		public override IMediaType ToDomain()
		{
			return new MediaType(Number, Name);
		}

		#endregion
	}
}