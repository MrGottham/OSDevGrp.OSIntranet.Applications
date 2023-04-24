using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Lending
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
		public void DisallowDeletion_WhenCalled_AssertDeletableIsFalseOnLending(bool deletable)
		{
			IDeletable sut = CreateSut(deletable);

			Assert.That(sut.Deletable, Is.EqualTo(deletable));

			sut.DisallowDeletion();

			Assert.That(sut.Deletable, Is.False);
		}

		private IDeletable CreateSut(bool deletable)
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;

			return new Domain.MediaLibrary.Lending(Guid.NewGuid(), _fixture.BuildBorrowerMock().Object, _fixture.BuildMediaMock().Object, lendingDate, recallDate, _random.Next(100) > 50 ? recallDate.AddDays(_random.Next(-7, 7)).Date : null, deletable);
		}
	}
}