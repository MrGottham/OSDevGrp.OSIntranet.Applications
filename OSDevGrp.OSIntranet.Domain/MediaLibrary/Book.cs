using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	public class Book : MediaBase, IBook
	{
		#region Constructors

		public Book(Guid mediaIdentifier, string title, string subtitle, string description, string details, IBookGenre bookGenre, ILanguage writtenLanguage, IMediaType mediaType, string internationalStandardBookNumber, short? published, Uri url, byte[] image, IEnumerable<IMediaPersonality> authors, Func<IMedia, IEnumerable<ILending>> lendingsBuilder)
			: this(mediaIdentifier, title, subtitle, description, details, bookGenre, writtenLanguage, mediaType, internationalStandardBookNumber, published, url, image, _ => Array.Empty<IMediaBinding>(), lendingsBuilder)
		{
			NullGuard.NotNull(authors, nameof(authors));

			foreach (IMediaPersonality author in authors)
			{
				MediaBindings.Add(author.AsAuthor(this));
			}
		}

		public Book(Guid mediaIdentifier, string title, string subtitle, string description, string details, IBookGenre bookGenre, ILanguage writtenLanguage, IMediaType mediaType, string internationalStandardBookNumber, short? published, Uri url, byte[] image, Func<IMedia, IEnumerable<IMediaBinding>> mediaBindingsBuilder, Func<IMedia, IEnumerable<ILending>> lendingsBuilder) 
			: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindingsBuilder, lendingsBuilder)
		{
			NullGuard.NotNull(bookGenre, nameof(bookGenre));

			BookGenre = bookGenre;
			WrittenLanguage = writtenLanguage;
			InternationalStandardBookNumber = internationalStandardBookNumber;
		}

		#endregion

		#region Properties

		public IBookGenre BookGenre { get; }

		public ILanguage WrittenLanguage { get; }

		public string InternationalStandardBookNumber { get; }

		public IEnumerable<IMediaPersonality> Authors => MediaBindings.Filter(MediaRole.Author);

		#endregion
	}
}