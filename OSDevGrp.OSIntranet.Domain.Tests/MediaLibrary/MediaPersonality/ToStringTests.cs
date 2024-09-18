using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.MediaPersonality
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
		[TestCase(true, true)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(false, false)]
		public void ToString_WhenCalled_ReturnsNotNull(bool hasGivenName, bool hasMiddleName)
		{
			IMediaPersonality sut = CreateSut(hasGivenName, hasMiddleName: hasMiddleName);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(false, false)]
		public void ToString_WhenCalled_ReturnsNotEmpty(bool hasGivenName, bool hasMiddleName)
		{
			IMediaPersonality sut = CreateSut(hasGivenName, hasMiddleName: hasMiddleName);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaPersonalityHasNoGivenNameAndNoMiddleName_ReturnsFullName()
		{
			string surname = _fixture.Create<string>();
			IMediaPersonality sut = CreateSut(false, hasMiddleName: false, surname: surname);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo(surname));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaPersonalityHasGivenNameAndNoMiddleName_ReturnsFullName()
		{
			string givenName = _fixture.Create<string>();
			string surname = _fixture.Create<string>();
			IMediaPersonality sut = CreateSut(givenName: givenName, hasMiddleName: false, surname: surname);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{givenName} {surname}"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaPersonalityHasNoGivenNameButMiddleName_ReturnsFullName()
		{
			string middleName = _fixture.Create<string>();
			string surname = _fixture.Create<string>();
			IMediaPersonality sut = CreateSut(false, middleName: middleName, surname: surname);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{middleName} {surname}"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenMediaPersonalityHasGivenNameAndMiddleName_ReturnsFullName()
		{
			string givenName = _fixture.Create<string>();
			string middleName = _fixture.Create<string>();
			string surname = _fixture.Create<string>();
			IMediaPersonality sut = CreateSut(givenName: givenName, middleName: middleName, surname: surname);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{givenName} {middleName} {surname}"));
		}

		private IMediaPersonality CreateSut(bool hasGivenName = true, string givenName = null, bool hasMiddleName = true, string middleName = null, string surname = null)
		{
			return new Domain.MediaLibrary.MediaPersonality(Guid.NewGuid(), hasGivenName ? givenName ?? _fixture.Create<string>() : null, hasMiddleName ? middleName ?? _fixture.Create<string>() : null, surname ?? _fixture.Create<string>(), _fixture.BuildNationalityMock().Object, _fixture.CreateMany<MediaRole>(1).ToArray(), _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.Create<DateTime>() : null, _random.Next(100) > 50 ? _fixture.CreateEndpoint(path: $"api/mediapersonalities/{Guid.NewGuid():D}") : null, _random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : null);
		}
	}
}