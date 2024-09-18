using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaPersonality
{
    [TestFixture]
	public class AllowDeletionTests
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
		public void AllowDeletion_WhenCalled_AssertDeletableIsTrueOnMediaPersonality(bool deletable)
		{
			IDeletable sut = CreateSut(deletable);

			Assert.That(sut.Deletable, Is.EqualTo(deletable));

			sut.AllowDeletion();

			Assert.That(sut.Deletable, Is.True);
		}

		private IDeletable CreateSut(bool deletable)
		{
			return new Domain.MediaLibrary.MediaPersonality(Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _fixture.BuildNationalityMock().Object, _fixture.CreateMany<MediaRole>(1).ToArray(), _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.CreateEndpoint(path: $"api/mediapersonalities/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null, deletable);
		}
	}
}