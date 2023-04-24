using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Borrower
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
		public void GetHashCode_WhenCalled_ReturnsHashCodeForBorrowerIdentifier()
		{
			Guid borrowerIdentifier = Guid.NewGuid();
			IBorrower sut = CreateSut(borrowerIdentifier);

			int result = sut.GetHashCode();

			Assert.That(result, Is.EqualTo(borrowerIdentifier.GetHashCode()));
		}

		private IBorrower CreateSut(Guid? borrowerIdentifier = null)
		{
			return new Domain.MediaLibrary.Borrower(borrowerIdentifier ?? Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _fixture.Create<string>(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(100) > 50 ? _fixture.Create<string>() : null, _random.Next(1, 3) * 7, _ => Array.Empty<ILending>());
		}
	}
}