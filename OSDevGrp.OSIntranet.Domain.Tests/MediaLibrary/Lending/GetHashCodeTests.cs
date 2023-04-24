using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Lending
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
		public void GetHashCode_WhenCalled_ReturnsHashCodeForLendingIdentifier()
		{
			Guid lendingIdentifier = Guid.NewGuid();
			ILending sut = CreateSut(lendingIdentifier);

			int result = sut.GetHashCode();

			Assert.That(result, Is.EqualTo(lendingIdentifier.GetHashCode()));
		}

		private ILending CreateSut(Guid? lendingIdentifier = null)
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;

			return new Domain.MediaLibrary.Lending(lendingIdentifier ?? Guid.NewGuid(), _fixture.BuildBorrowerMock().Object, _fixture.BuildMediaMock().Object, lendingDate, recallDate, _random.Next(100) > 50 ? recallDate.AddDays(_random.Next(-7, 7)).Date : null);
		}
	}
}