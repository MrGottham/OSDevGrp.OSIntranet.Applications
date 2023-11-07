using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IBorrowerDataCommand : IBorrowerIdentificationCommand
	{
		string FullName { get; }

		string MailAddress { get; }

		string PrimaryPhone { get; }

		string SecondaryPhone { get; }

		int LendingLimit { get; }

		Task<IBorrower> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository);
	}
}