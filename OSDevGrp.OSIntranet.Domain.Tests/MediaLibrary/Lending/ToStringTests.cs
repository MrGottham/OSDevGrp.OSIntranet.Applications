using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Lending
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
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_AssertToStringWasCalledOnBorrower(bool returned)
		{
			Mock<IBorrower> borrowerMock = _fixture.BuildBorrowerMock();
			ILending sut = CreateSut(borrower: borrowerMock.Object, returned: returned);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.ToString();
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			borrowerMock.Verify(m => m.ToString(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_AssertToStringWasCalledOnMedia(bool returned)
		{
			Mock<IMedia> mediaMock = _fixture.BuildMediaMock();
			ILending sut = CreateSut(media: mediaMock.Object, returned: returned);

			// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			sut.ToString();
			// ReSharper restore ReturnValueOfPureMethodIsNotUsed

			mediaMock.Verify(m => m.ToString(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsNotNull(bool returned)
		{
			ILending sut = CreateSut(returned: returned);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsNotEmpty(bool returned)
		{
			ILending sut = CreateSut(returned: returned);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsStringContainingToStringValueFromBorrower(bool returned)
		{
			string toString = _fixture.Create<string>();
			IBorrower borrower = _fixture.BuildBorrowerMock(toString: toString).Object;
			ILending sut = CreateSut(borrower: borrower, returned: returned);

			string result = sut.ToString();

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Contains(toString), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsStringContainingToStringValueFromMedia(bool returned)
		{
			string toString = _fixture.Create<string>();
			IMedia media = _fixture.BuildMediaMock(toString: toString).Object;
			ILending sut = CreateSut(media: media, returned: returned);

			string result = sut.ToString();

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Contains(toString), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsStringContainingLendingDate(bool returned)
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			ILending sut = CreateSut(lendingDate: lendingDate, returned: returned);

			string result = sut.ToString();

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Contains(lendingDate.ToString("D", CultureInfo.InvariantCulture)), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenCalled_ReturnsStringContainingRecallDate(bool returned)
		{
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			ILending sut = CreateSut(recallDate: recallDate, returned: returned);

			string result = sut.ToString();

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Contains(recallDate.ToString("D", CultureInfo.InvariantCulture)), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenLendingDoesNotHaveReturnedDate_ReturnsStringForLending()
		{
			string borrowerToString = _fixture.Create<string>();
			IBorrower borrower = _fixture.BuildBorrowerMock(toString: borrowerToString).Object;
			string mediaToString = _fixture.Create<string>();
			IMedia media = _fixture.BuildMediaMock(toString: mediaToString).Object;
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;
			ILending sut = CreateSut(borrower, media, lendingDate, recallDate);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"Borrower: {borrowerToString}{Environment.NewLine}Media: {mediaToString}{Environment.NewLine}Lending date: {lendingDate.ToString("D", CultureInfo.InvariantCulture)}{Environment.NewLine}Recall date: {recallDate.ToString("D", CultureInfo.InvariantCulture)}"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenLendingHasReturnedDate_ReturnsStringContainingReturnedDate()
		{
			string borrowerToString = _fixture.Create<string>();
			IBorrower borrower = _fixture.BuildBorrowerMock(toString: borrowerToString).Object;
			string mediaToString = _fixture.Create<string>();
			IMedia media = _fixture.BuildMediaMock(toString: mediaToString).Object;
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;
			DateTime returnedDate = recallDate.AddDays(_random.Next(-7, 7)).Date;
			ILending sut = CreateSut(borrower, media, lendingDate, recallDate, true, returnedDate);

			string result = sut.ToString();

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Contains(returnedDate.ToString("D", CultureInfo.InvariantCulture)), Is.True);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenLendingHasReturnedDate_ReturnsStringForLending()
		{
			string borrowerToString = _fixture.Create<string>();
			IBorrower borrower = _fixture.BuildBorrowerMock(toString: borrowerToString).Object;
			string mediaToString = _fixture.Create<string>();
			IMedia media = _fixture.BuildMediaMock(toString: mediaToString).Object;
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			DateTime recallDate = lendingDate.AddDays(_random.Next(14, 21)).Date;
			DateTime returnedDate = recallDate.AddDays(_random.Next(-7, 7)).Date;
			ILending sut = CreateSut(borrower, media, lendingDate, recallDate, true, returnedDate);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"Borrower: {borrowerToString}{Environment.NewLine}Media: {mediaToString}{Environment.NewLine}Lending date: {lendingDate.ToString("D", CultureInfo.InvariantCulture)}{Environment.NewLine}Recall date: {recallDate.ToString("D", CultureInfo.InvariantCulture)}{Environment.NewLine}Returned: {returnedDate.ToString("D", CultureInfo.InvariantCulture)}"));
		}

		private ILending CreateSut(IBorrower borrower = null, IMedia media = null, DateTime? lendingDate = null, DateTime? recallDate = null, bool returned = false, DateTime? returnedDate = null)
		{
			lendingDate ??= DateTime.Today.AddDays(_random.Next(0, 365) * -1).Date;
			recallDate ??= lendingDate.Value.AddDays(14).Date;
			returnedDate ??= returned ? recallDate.Value.AddDays(_random.Next(-7, 7)).Date : null;

			return new Domain.MediaLibrary.Lending(Guid.NewGuid(), borrower ?? _fixture.BuildBorrowerMock().Object, media ?? _fixture.BuildMediaMock().Object, lendingDate.Value, recallDate.Value, returnedDate);
		}
	}
}