using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Borrower
{
	[TestFixture]
	public class DisallowDeletionTests
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
		public void DisallowDeletion_WhenCalled_AssertDeletableIsFalseOnBorrower(bool deletable)
		{
			IDeletable sut = CreateSut(deletable);

			Assert.That(sut.Deletable, Is.EqualTo(deletable));

			sut.DisallowDeletion();

			Assert.That(sut.Deletable, Is.False);
		}

		private IDeletable CreateSut(bool deletable)
		{
			return new Domain.MediaLibrary.Borrower(Guid.NewGuid(), _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(1, 3) * 7, _ => Array.Empty<ILending>(), deletable);
		}
	}
}