using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetMediaPersonalitiesAsyncTests : MediaLibraryRepositoryTestBase
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
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsNull_ReturnsNonEmptyCollectionOfMediaPersonalities()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsEmpty_ReturnsNonEmptyCollectionOfMediaPersonalities()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(string.Empty);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterIsWhiteSpace_ReturnsNonEmptyCollectionOfMediaPersonalities()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(" ");

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterMatchesOneOrMorePersonalities_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality knownMediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			string nameFilter = knownMediaPersonality.Surname.Substring(0, _random.Next(1, knownMediaPersonality.Surname.Length - 1));
			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(nameFilter);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterMatchesOneOrMorePersonalities_ReturnsNonEmptyCollectionOfMediaPersonalities()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality knownMediaPersonality = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			string nameFilter = knownMediaPersonality.Surname.Substring(0, _random.Next(1, knownMediaPersonality.Surname.Length - 1));
			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(nameFilter);

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterDoesNotMatchOneOrMorePersonalities_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenNameFilterDoesNotMatchOneOrMorePersonalities_ReturnsEmptyCollection()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}
	}
}