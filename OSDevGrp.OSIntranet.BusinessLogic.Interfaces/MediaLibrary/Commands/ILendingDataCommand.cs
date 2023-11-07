using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface ILendingDataCommand : ILendingIdentificationCommand
	{
		Guid BorrowerIdentifier { get; }

		Guid MediaIdentifier { get; }

		DateTime LendingDate { get; }

		DateTime RecallDate { get; }

		DateTime? ReturnedDate { get; }

		Task<ILending> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository);
	}
}