﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaBase
{
	[TestFixture]
	public class ToStringTests
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
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsNotNull(bool hasSubtitle)
		{
			IMedia sut = CreateSut(hasSubtitle: hasSubtitle);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsNotEmpty(bool hasSubtitle)
		{
			IMedia sut = CreateSut(hasSubtitle: hasSubtitle);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaHasNoSubtitle_ReturnsFullTitle()
		{
			string title = _fixture.Create<string>();
			IMedia sut = CreateSut(title, false);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo(title));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaHasSubtitle_ReturnsFullTitle()
		{
			string title = _fixture.Create<string>();
			string subtitle = _fixture.Create<string>();
			IMedia sut = CreateSut(title, subtitle: subtitle);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{title}, {subtitle}"));
		}

		private IMedia CreateSut(string title = null, bool hasSubtitle = true, string subtitle = null)
		{
			return new MyMedia(Guid.NewGuid(), title ?? _fixture.Create<string>(), hasSubtitle ? subtitle ?? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.BuildMediaTypeMock().Object, _random.Next(100) > 50 ? null : _fixture.Create<short>(), _random.Next(100) > 50 ? new Uri($"https://localhost/api/medias/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, Array.Empty<IMediaBinding>());
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