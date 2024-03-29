﻿using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class Music : MediaBase, IMusic
	{
		#region Constructors

		public Music(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMusicGenre musicGenre, IMediaType mediaType, short? published, short? tracks, Uri url, byte[] image, IEnumerable<IMediaPersonality> artists, Func<IMedia, IEnumerable<ILending>> lendingsBuilder)
			: this(mediaIdentifier, title, subtitle, description, details, musicGenre, mediaType, published, tracks, url, image, _ => Array.Empty<IMediaBinding>(), lendingsBuilder)
		{
			NullGuard.NotNull(artists, nameof(artists));

			foreach (IMediaPersonality artist in artists)
			{
				MediaBindings.Add(artist.AsArtist(this));
			}
		}

		public Music(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMusicGenre musicGenre, IMediaType mediaType, short? published, short? tracks, Uri url, byte[] image, Func<IMedia, IEnumerable<IMediaBinding>> mediaBindingsBuilder, Func<IMedia, IEnumerable<ILending>> lendingsBuilder)
			: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindingsBuilder, lendingsBuilder)
		{
			NullGuard.NotNull(musicGenre, nameof(musicGenre));

			MusicGenre = musicGenre;
			Tracks = tracks;
		}

		#endregion

		#region Properties

		public IMusicGenre MusicGenre { get; }

		public short? Tracks { get; }

		public IEnumerable<IMediaPersonality> Artists => MediaBindings.Filter(MediaRole.Artist);

		#endregion
	}
}