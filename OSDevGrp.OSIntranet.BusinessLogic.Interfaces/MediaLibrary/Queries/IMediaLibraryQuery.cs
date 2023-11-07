using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface IMediaLibraryQuery : IQuery
	{
		IValidator Validate(IValidator validator, IMediaLibraryRepository mediaLibraryRepository);
	}
}