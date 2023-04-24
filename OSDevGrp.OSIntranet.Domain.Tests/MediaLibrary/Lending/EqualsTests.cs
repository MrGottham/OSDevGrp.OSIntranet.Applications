using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Lending
{
	[TestFixture]
	public class EqualsTests
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
		public void Equals_WhenObjectIsLending_AssertLendingIdentifierWasCalledOnLending()
		{
			ILending sut = CreateSut();

			Mock<ILending> lendingMock = _fixture.BuildLendingMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(lendingMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			lendingMock.Verify(m => m.LendingIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNull_ReturnsFalse()
		{
			ILending sut = CreateSut();

			bool result = sut.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNotLending_ReturnsFalse()
		{
			ILending sut = CreateSut();

			bool result = sut.Equals(_fixture.Create<object>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsLendingWithNonMatchingLendingIdentifier_ReturnsFalse()
		{
			ILending sut = CreateSut();

			bool result = sut.Equals(_fixture.BuildLendingMock().Object);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsLendingWithMatchingLendingIdentifier_ReturnsTrue()
		{
			Guid lendingIdentifier = Guid.NewGuid();
			ILending sut = CreateSut(lendingIdentifier);

			bool result = sut.Equals(_fixture.BuildLendingMock(lendingIdentifier).Object);

			Assert.That(result, Is.True);
		}

		private ILending CreateSut(Guid? lendingIdentifier = null)
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;

			return new Domain.MediaLibrary.Lending(lendingIdentifier ?? Guid.NewGuid(), _fixture.BuildBorrowerMock().Object, _fixture.BuildMediaMock().Object, lendingDate, recallDate, _random.Next(100) > 50 ? recallDate.AddDays(_random.Next(-7, 7)).Date : null);
		}
	}
}