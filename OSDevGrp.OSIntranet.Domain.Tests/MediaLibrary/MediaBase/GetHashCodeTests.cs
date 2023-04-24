using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBase
{
	[TestFixture]
	public class GetHashCodeTests
	{
		#region Private variables

		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void GetHashCode_WhenCalled_ReturnsHashCodeForMediaIdentifier()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			IMedia sut = CreateSut(mediaIdentifier);

			int result = sut.GetHashCode();

			Assert.That(result, Is.EqualTo(mediaIdentifier.GetHashCode()));
		}

		private IMedia CreateSut(Guid? mediaIdentifier = null)
		{
			return new MyMedia(mediaIdentifier ?? Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(), _random.Next(100) > 50 ? new Uri($"https://localhost/api/medias/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, _ => Array.Empty<IMediaBinding>(), _ => Array.Empty<ILending>());
		}

		private class MyMedia : Domain.MediaLibrary.MediaBase
		{
			#region Constructor

			public MyMedia(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMediaType mediaType, short? published, Uri url, byte[] image, Func<IMedia, IEnumerable<IMediaBinding>> mediaBindingsBuilder, Func<IMedia, IEnumerable<ILending>> lendingsBuilder)
				: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindingsBuilder, lendingsBuilder)
			{
			}

			#endregion
		}
	}
}