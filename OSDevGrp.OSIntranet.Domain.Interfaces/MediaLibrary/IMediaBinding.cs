using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IMediaBinding : IAuditable, IDeletable
	{
		IMedia Media { get; }

		MediaRole Role { get; }

		IMediaPersonality MediaPersonality { get; }
	}
}