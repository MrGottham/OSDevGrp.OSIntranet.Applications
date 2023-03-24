using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class Movie : MediaBase, IMovie
	{
		#region Constructors

		public Movie(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMovieGenre movieGenre, ILanguage spokenLanguage, IMediaType mediaType, short? published, short? length, Uri url, byte[] image, IEnumerable<IMediaPersonality> directors, IEnumerable<IMediaPersonality> actors)
			: this(mediaIdentifier, title, subtitle, description, details, movieGenre, spokenLanguage, mediaType, published, length, url, image, Array.Empty<IMediaBinding>())
		{
			NullGuard.NotNull(directors, nameof(directors))
				.NotNull(actors, nameof(actors));

			foreach (IMediaPersonality director in directors)
			{
				MediaBindings.Add(director.AsDirector(this));
			}

			foreach (IMediaPersonality actor in actors)
			{
				MediaBindings.Add(actor.AsActor(this));
			}
		}

		public Movie(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMovieGenre movieGenre, ILanguage spokenLanguage, IMediaType mediaType, short? published, short? length, Uri url, byte[] image, IEnumerable<IMediaBinding> mediaBindings) 
			: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindings)
		{
			NullGuard.NotNull(movieGenre, nameof(movieGenre));

			MovieGenre = movieGenre;
			SpokenLanguage = spokenLanguage;
			Length = length;
		}

		#endregion

		#region Properties

		public IMovieGenre MovieGenre { get; }

		public ILanguage SpokenLanguage { get; }

		public short? Length { get; }

		public IEnumerable<IMediaPersonality> Directors => MediaBindings.Filter(MediaRole.Director);

		public IEnumerable<IMediaPersonality> Actors => MediaBindings.Filter(MediaRole.Actor);

		#endregion
	}
}