using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaPersonalityResolver
{
	[TestFixture]
	public class ResolveAsyncTests
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
		public void ResolveAsync_WhenMediaPersonalityIdentifiersIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(null, CreateMediaLibraryRepository()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaPersonalityIdentifiers"));
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(CreateMediaPersonalityIdentifiers(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenCalled_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepositoryForEachUniqueMediaPersonalityIdentifier()
		{
			Guid[] mediaPersonalityIdentifiersToDuplicate = CreateMediaPersonalityIdentifiers().ToArray();

			List<Guid> mediaPersonalityIdentifiers = new List<Guid>(mediaPersonalityIdentifiersToDuplicate);
			mediaPersonalityIdentifiers.AddRange(mediaPersonalityIdentifiersToDuplicate);
			mediaPersonalityIdentifiers.AddRange(mediaPersonalityIdentifiersToDuplicate);

			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = CreateMediaLibraryRepositoryMock();

			await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(mediaPersonalityIdentifiers, mediaLibraryRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in mediaPersonalityIdentifiers)
			{
				mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalityAsync(It.Is<Guid>(value => value == mediaPersonalityIdentifier)), Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenAllMediaPersonalityIdentifiesExists_ReturnsNotNull()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(existingMediaPersonalityIdentifies, CreateMediaLibraryRepository());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenAllMediaPersonalityIdentifiesExists_ReturnsNonEmptyMediaPersonalityCollection()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(existingMediaPersonalityIdentifies, CreateMediaLibraryRepository());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenAllMediaPersonalityIdentifiesExists_ReturnsMediaPersonalityCollectionWithExistingMediaPersonalities()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(existingMediaPersonalityIdentifies, CreateMediaLibraryRepository());

			Assert.That(existingMediaPersonalityIdentifies.All(mediaPersonalityIdentifier => result.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenSomeMediaPersonalityIdentifiesExists_ReturnsNotNull()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] mediaPersonalityIdentifiers = existingMediaPersonalityIdentifies.Concat(nonExistingMediaPersonalityIdentifies).ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(mediaPersonalityIdentifiers, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenSomeMediaPersonalityIdentifiesExists_ReturnsNonEmptyMediaPersonalityCollection()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] mediaPersonalityIdentifiers = existingMediaPersonalityIdentifies.Concat(nonExistingMediaPersonalityIdentifies).ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(mediaPersonalityIdentifiers, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenSomeMediaPersonalityIdentifiesExists_ReturnsMediaPersonalityCollectionWithExistingMediaPersonalities()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] mediaPersonalityIdentifiers = existingMediaPersonalityIdentifies.Concat(nonExistingMediaPersonalityIdentifies).ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(mediaPersonalityIdentifiers, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(existingMediaPersonalityIdentifies.All(mediaPersonalityIdentifier => result.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) != null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenSomeMediaPersonalityIdentifiesExists_ReturnsMediaPersonalityCollectionWithoutNonExistingMediaPersonalities()
		{
			Guid[] existingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();
			Guid[] mediaPersonalityIdentifiers = existingMediaPersonalityIdentifies.Concat(nonExistingMediaPersonalityIdentifies).ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(mediaPersonalityIdentifiers, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(nonExistingMediaPersonalityIdentifies.All(mediaPersonalityIdentifier => result.SingleOrDefault(mediaPersonality => mediaPersonality.MediaPersonalityIdentifier == mediaPersonalityIdentifier) == null), Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenNoneMediaPersonalityIdentifiesExists_ReturnsNotNull()
		{
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(nonExistingMediaPersonalityIdentifies, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ResolveAsync_WhenNoneMediaPersonalityIdentifiesExists_ReturnsEmptyMediaPersonalityCollection()
		{
			Guid[] nonExistingMediaPersonalityIdentifies = CreateMediaPersonalityIdentifiers().ToArray();

			IEnumerable<IMediaPersonality> result = await BusinessLogic.MediaLibrary.Logic.MediaPersonalityResolver.ResolveAsync(nonExistingMediaPersonalityIdentifies, CreateMediaLibraryRepository(nonExistingMediaPersonalityIdentifies));

			Assert.That(result, Is.Empty);
		}

		private IEnumerable<Guid> CreateMediaPersonalityIdentifiers()
		{
			return _fixture.CreateMany<Guid>(_random.Next(5, 25)).ToArray();
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(params Guid[] nonExistingMediaPersonalityIdentifiers)
		{
			NullGuard.NotNull(nonExistingMediaPersonalityIdentifiers, nameof(nonExistingMediaPersonalityIdentifiers));

			return CreateMediaLibraryRepositoryMock(nonExistingMediaPersonalityIdentifiers).Object;
		}

		private Mock<IMediaLibraryRepository> CreateMediaLibraryRepositoryMock(params Guid[] nonExistingMediaPersonalityIdentifiers)
		{
			NullGuard.NotNull(nonExistingMediaPersonalityIdentifiers, nameof(nonExistingMediaPersonalityIdentifiers));

			Mock<IMediaLibraryRepository> mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns<Guid>(mediaPersonalityIdentifier => Task.FromResult(nonExistingMediaPersonalityIdentifiers.Contains(mediaPersonalityIdentifier) == false ? _fixture.BuildMediaPersonalityMock(mediaPersonalityIdentifier).Object : null));
			return mediaLibraryRepositoryMock;
		}
	}
}