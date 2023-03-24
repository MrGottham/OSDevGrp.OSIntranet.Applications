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
	public class GetMediaBindingsTests
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
		public void GetMediaBindings_WhenMediaHasNoMediaBindings_ReturnsNotNull()
		{
			IMedia sut = CreateSut(false);

			IEnumerable<IMediaBinding> result = sut.GetMediaBindings();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaBindings_WhenMediaHasNoMediaBindings_ReturnsEmptyCollectionOfMediaBindings()
		{
			IMedia sut = CreateSut(false);

			IEnumerable<IMediaBinding> result = sut.GetMediaBindings();

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaBindings_WhenMediaHasMediaBindings_ReturnsNotNull()
		{
			IMedia sut = CreateSut();

			IEnumerable<IMediaBinding> result = sut.GetMediaBindings();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaBindings_WhenMediaHasMediaBindings_ReturnsNonEmptyCollectionOfMediaBindings()
		{
			IMedia sut = CreateSut();

			IEnumerable<IMediaBinding> result = sut.GetMediaBindings();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMediaBindings_WhenMediaHasMediaBindings_ReturnsCollectionOfMediaBindingsFromMedia()
		{
			IMediaBinding[] mediaBindings = 
			{
				_fixture.BuildMediaBindingMock().Object,
				_fixture.BuildMediaBindingMock().Object,
				_fixture.BuildMediaBindingMock().Object
			};
			IMedia sut = CreateSut(mediaBindings: mediaBindings);

			IEnumerable<IMediaBinding> result = sut.GetMediaBindings();

			Assert.That(result, Is.EqualTo(mediaBindings));
		}

		private IMedia CreateSut(bool hasMediaBindings = true, IEnumerable<IMediaBinding> mediaBindings = null )
		{
			return new MyMedia(Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(), _random.Next(100) > 50 ? new Uri($"https://localhost/api/medias/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, hasMediaBindings ? mediaBindings ?? new[] { _fixture.BuildMediaBindingMock().Object, _fixture.BuildMediaBindingMock().Object, _fixture.BuildMediaBindingMock().Object } : Array.Empty<IMediaBinding>());
		}

		private class MyMedia : Domain.MediaLibrary.MediaBase
		{
			#region Constructor

			public MyMedia(Guid mediaIdentifier, string title, string subtitle, string description, string details, IMediaType mediaType, short? published, Uri url, byte[] image, IEnumerable<IMediaBinding> mediaBindings)
				: base(mediaIdentifier, title, subtitle, description, details, mediaType, published, url, image, mediaBindings)
			{
			}

			#endregion
		}
	}
}