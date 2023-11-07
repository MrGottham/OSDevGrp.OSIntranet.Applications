using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IUpdateMusicGenreCommand : IUpdateGenericCategoryCommand<IMusicGenre>
	{
	}
}