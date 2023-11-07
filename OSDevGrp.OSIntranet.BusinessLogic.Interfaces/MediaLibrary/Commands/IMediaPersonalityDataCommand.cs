using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMediaPersonalityDataCommand : IMediaPersonalityIdentificationCommand
	{
		string GivenName { get; }

		string MiddleName { get; }

		string Surname { get; }

		int NationalityIdentifier { get; }

		DateTime? BirthDate { get; }

		DateTime? DateOfDead { get; }

		string Url { get; }

		byte[] Image { get; }

		Task<IMediaPersonality> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository);
	}
}