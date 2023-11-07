using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaPersonality
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
		public void GetHashCode_WhenCalled_ReturnsHashCodeForMediaPersonalityIdentifier()
		{
			Guid mediaPersonalityIdentifier = Guid.NewGuid();
			IMediaPersonality sut = CreateSut(mediaPersonalityIdentifier);

			int result = sut.GetHashCode();

			Assert.That(result, Is.EqualTo(mediaPersonalityIdentifier.GetHashCode()));
		}

		private IMediaPersonality CreateSut(Guid? mediaPersonalityIdentifier = null)
		{
			return new Domain.MediaLibrary.MediaPersonality(mediaPersonalityIdentifier ?? Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _fixture.BuildNationalityMock().Object, _fixture.CreateMany<MediaRole>(1).ToArray(), _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? new Uri($"https://localhost/api/mediapersonalities/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null);
		}
	}
}