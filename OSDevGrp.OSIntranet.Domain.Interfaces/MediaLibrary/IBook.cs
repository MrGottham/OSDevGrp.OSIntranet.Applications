using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IBook : IMedia
	{
		IBookGenre BookGenre { get; }

		ILanguage WrittenLanguage { get; }

		string InternationalStandardBookNumber { get; }

		IEnumerable<IMediaPersonality> Authors { get; }
	}
}