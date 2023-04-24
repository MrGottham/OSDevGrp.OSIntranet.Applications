using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Borrower
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
		public void Equals_WhenObjectIsBorrower_AssertBorrowerIdentifierWasCalledOnBorrower()
		{
			IBorrower sut = CreateSut();

			Mock<IBorrower> borrowerMock = _fixture.BuildBorrowerMock();
			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.Equals(borrowerMock.Object);
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			borrowerMock.Verify(m => m.BorrowerIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNull_ReturnsFalse()
		{
			IBorrower sut = CreateSut();

			bool result = sut.Equals(null);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsNotBorrower_ReturnsFalse()
		{
			IBorrower sut = CreateSut();

			bool result = sut.Equals(_fixture.Create<object>());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsBorrowerWithNonMatchingBorrowerIdentifier_ReturnsFalse()
		{
			IBorrower sut = CreateSut();

			bool result = sut.Equals(_fixture.BuildBorrowerMock().Object);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void Equals_WhenObjectIsBorrowerWithMatchingBorrowerIdentifier_ReturnsTrue()
		{
			Guid borrowerIdentifier = Guid.NewGuid();
			IBorrower sut = CreateSut(borrowerIdentifier);

			bool result = sut.Equals(_fixture.BuildBorrowerMock(borrowerIdentifier).Object);

			Assert.That(result, Is.True);
		}

		private IBorrower CreateSut(Guid? borrowerIdentifier = null)
		{
			return new Domain.MediaLibrary.Borrower(borrowerIdentifier ?? Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(1, 3) * 7, _ => Array.Empty<ILending>());
		}
	}
}