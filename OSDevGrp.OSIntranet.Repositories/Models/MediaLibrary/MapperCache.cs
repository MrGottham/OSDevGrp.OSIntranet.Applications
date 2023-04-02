using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MapperCache
	{
		#region Properties

		internal IDictionary<Guid, IMedia> MediaDictionary = new ConcurrentDictionary<Guid, IMedia>();

		internal IDictionary<Guid, IMediaPersonality> MediaPersonalityDictionary = new ConcurrentDictionary<Guid, IMediaPersonality>();

		internal object SyncRoot = new();

		#endregion
	}
}