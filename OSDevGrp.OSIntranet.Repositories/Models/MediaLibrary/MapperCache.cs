using OSDevGrp.OSIntranet.Core;
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

		internal IDictionary<Guid, IBorrower> BorrowerDictionary = new ConcurrentDictionary<Guid, IBorrower>();

		internal object SyncRoot = new();

		#endregion

		#region Methods

		internal void Cache(IMedia media)
		{
			NullGuard.NotNull(media, nameof(media));

			lock (SyncRoot)
			{
				if (MediaDictionary.ContainsKey(media.MediaIdentifier))
				{
					return;
				}

				MediaDictionary.Add(media.MediaIdentifier, media);
			}
		}

		internal void Cache(IMediaPersonality mediaPersonality)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality));

			lock (SyncRoot)
			{
				if (MediaPersonalityDictionary.ContainsKey(mediaPersonality.MediaPersonalityIdentifier))
				{
					return;
				}

				MediaPersonalityDictionary.Add(mediaPersonality.MediaPersonalityIdentifier, mediaPersonality);
			}
		}

		internal void Cache(IBorrower borrower)
		{
			NullGuard.NotNull(borrower, nameof(borrower));

			lock (SyncRoot)
			{
				if (BorrowerDictionary.ContainsKey(borrower.BorrowerIdentifier))
				{
					return;
				}

				BorrowerDictionary.Add(borrower.BorrowerIdentifier, borrower);
			}
		}

		#endregion
	}
}