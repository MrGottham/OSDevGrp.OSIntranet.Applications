using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.MediaLibrary.Borrower
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
		[TestCase(true, true, true)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(true, false, false)]
		[TestCase(false, true, true)]
		[TestCase(false, true, false)]
		[TestCase(false, false, true)]
		[TestCase(false, false, false)]
		public void ToString_WhenCalled_ReturnsNotNull(bool hasMailAddress, bool hasPrimaryPhone, bool hasSecondaryPhone)
		{
			IBorrower sut = CreateSut(hasMailAddress: hasMailAddress, hasPrimaryPhone: hasPrimaryPhone, hasSecondaryPhone: hasSecondaryPhone);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(true, false, false)]
		[TestCase(false, true, true)]
		[TestCase(false, true, false)]
		[TestCase(false, false, true)]
		[TestCase(false, false, false)]
		public void ToString_WhenCalled_ReturnsNotEmpty(bool hasMailAddress, bool hasPrimaryPhone, bool hasSecondaryPhone)
		{
			IBorrower sut = CreateSut(hasMailAddress: hasMailAddress, hasPrimaryPhone: hasPrimaryPhone, hasSecondaryPhone: hasSecondaryPhone);

			string result = sut.ToString();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenOnlyFullNameWasGivenOnBorrower_ReturnsStringContainingOnlyFullName()
		{
			string fullName = _fixture.Create<string>();
			IBorrower sut = CreateSut(fullName, hasMailAddress: false, hasPrimaryPhone: false, hasSecondaryPhone: false);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo(fullName));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(false, false)]
		public void ToString_WhenFullNameAndMailAddressWasGivenOnBorrower_ReturnsStringContainingFullNameAndMailAddress(bool hasPrimaryPhone, bool hasSecondaryPhone)
		{
			string fullName = _fixture.Create<string>();
			string mailAddress = _fixture.Create<string>();
			IBorrower sut = CreateSut(fullName, hasMailAddress: true, mailAddress: mailAddress, hasPrimaryPhone: hasPrimaryPhone, hasSecondaryPhone: hasSecondaryPhone);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{fullName} ({mailAddress})"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ToString_WhenFullNameAndNoMailAddressButPrimaryPhoneWasGivenOnBorrower_ReturnsStringContainingFullNameAndPrimaryPhone(bool hasSecondaryPhone)
		{
			string fullName = _fixture.Create<string>();
			string primaryPhone = _fixture.Create<string>();
			IBorrower sut = CreateSut(fullName, hasMailAddress: false, hasPrimaryPhone: true, primaryPhone: primaryPhone, hasSecondaryPhone: hasSecondaryPhone);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{fullName} ({primaryPhone})"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToString_WhenFullNameAndNoMailAddressAndNoPrimaryPhoneButSecondaryPhoneWasGivenOnBorrower_ReturnsStringContainingFullNameAndSecondaryPhone()
		{
			string fullName = _fixture.Create<string>();
			string secondaryPhone = _fixture.Create<string>();
			IBorrower sut = CreateSut(fullName, hasMailAddress: false, hasPrimaryPhone: false, hasSecondaryPhone: true, secondaryPhone: secondaryPhone);

			string result = sut.ToString();

			Assert.That(result, Is.EqualTo($"{fullName} ({secondaryPhone})"));
		}

		private IBorrower CreateSut(string fullName = null, bool hasMailAddress = true, string mailAddress = null, bool hasPrimaryPhone = true, string primaryPhone = null, bool hasSecondaryPhone = true, string secondaryPhone = null)
		{
			return new Domain.MediaLibrary.Borrower(Guid.NewGuid(), _random.Next(100) > 50 ? _fixture.Create<string>() : null, fullName ?? _fixture.Create<string>(), hasMailAddress ? mailAddress ?? _fixture.Create<string>() : null, hasPrimaryPhone ? primaryPhone ?? _fixture.Create<string>() : null, hasSecondaryPhone ? secondaryPhone ?? _fixture.Create<string>() : null, _random.Next(1, 3) * 7, _ => Array.Empty<ILending>());
		}
	}
}