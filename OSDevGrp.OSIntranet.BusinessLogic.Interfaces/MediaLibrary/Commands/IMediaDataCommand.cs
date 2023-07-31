using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMediaDataCommand<TMedia> : IMediaIdentificationCommand where TMedia : IMedia
	{
		string Title { get; }

		string Subtitle { get; }

		string Description { get; }

		string Details { get; }

		int MediaTypeIdentifier { get; }

		short? Published { get; }

		string Url { get; }

		byte[] Image { get; }

		Task<TMedia> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository);
	}
}