using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetBorrowersAsyncTests : MediaLibraryRepositoryTestBase
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
		public async Task GetBorrowersAsync_WhenFullNameFilterIsNull_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterIsNull_ReturnsEmptyCollectionOfBorrowers()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync();

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterIsEmpty_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(string.Empty);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterIsEmpty_ReturnsEmptyCollectionOfBorrowers()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(string.Empty);

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterIsWhiteSpace_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(" ");

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterIsWhiteSpace_ReturnsEmptyCollectionOfBorrowers()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(" ");

			Assert.That(result, Is.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterMatchesOneOrMoreBorrowers_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBorrower[] knownBorrowers = (await sut.GetBorrowersAsync()).ToArray();
			if (knownBorrowers.Any() == false)
			{
				return;
			}

			IBorrower knownBorrower = knownBorrowers[_random.Next(0, knownBorrowers.Length - 1)];
			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(knownBorrower.FullName.Substring(0, _random.Next(1, knownBorrower.FullName.Length - 1)));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterMatchesOneOrMoreBorrowers_ReturnsNonEmptyCollectionOfBorrowers()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBorrower[] knownBorrowers = (await sut.GetBorrowersAsync()).ToArray();
			if (knownBorrowers.Any() == false)
			{
				return;
			}

			IBorrower knownBorrower = knownBorrowers[_random.Next(0, knownBorrowers.Length - 1)];
			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(knownBorrower.FullName.Substring(0, _random.Next(1, knownBorrower.FullName.Length - 1)));

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterDoesNotMatchOneOrMoreBorrowers_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowersAsync_WhenFullNameFilterDoesNotMatchOneOrMoreBorrowers_ReturnsEmptyCollectionOfBorrowers()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IBorrower> result = await sut.GetBorrowersAsync(_fixture.Create<string>());

			Assert.That(result, Is.Empty);
		}
	}
}