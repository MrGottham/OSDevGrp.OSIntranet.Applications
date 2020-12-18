using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.Contact
{
    [TestFixture]
    public class HasBirthdayWithinDaysTests
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
        [TestCase(0)]
        [TestCase(7)]
        [TestCase(14)]
        [TestCase(21)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithoutBirthday_ReturnsFalse(int withinDays)
        {
            IContact sut = CreateSut(false);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(0)]
        [TestCase(7)]
        [TestCase(14)]
        [TestCase(21)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithBirthdayBeforeTodayThisYear_ReturnsFalse(int withinDays)
        {
            DateTime birthday = DateTime.Today.AddDays(-1).AddYears(_random.Next(25, 50) * -1);
            IContact sut = CreateSut(birthday: birthday);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(0)]
        [TestCase(7)]
        [TestCase(14)]
        [TestCase(21)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithBirthdayTodayThisYear_ReturnsTrue(int withinDays)
        {
            DateTime birthday = DateTime.Today.AddYears(_random.Next(25, 50) * -1);
            IContact sut = CreateSut(birthday: birthday);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(0, false)]
        [TestCase(7, true)]
        [TestCase(14, true)]
        [TestCase(21, true)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithBirthdayOneWeekAfterTodayThisYear_ReturnsExpectedResult(int withinDays, bool expectedResult)
        {
            DateTime birthday = DateTime.Today.AddDays(7).AddYears(_random.Next(25, 50) * -1);
            IContact sut = CreateSut(birthday: birthday);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(0, false)]
        [TestCase(7, false)]
        [TestCase(14, true)]
        [TestCase(21, true)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithBirthdayTwoWeekAfterTodayThisYear_ReturnsExpectedResult(int withinDays, bool expectedResult)
        {
            DateTime birthday = DateTime.Today.AddDays(14).AddYears(_random.Next(25, 50) * -1);
            IContact sut = CreateSut(birthday: birthday);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(0, false)]
        [TestCase(7, false)]
        [TestCase(14, false)]
        [TestCase(21, true)]
        public void HasBirthdayWithinDays_WhenCalledOnContactWithBirthdayThreeWeekAfterTodayThisYear_ReturnsExpectedResult(int withinDays, bool expectedResult)
        {
            DateTime birthday = DateTime.Today.AddDays(21).AddYears(_random.Next(25, 50) * -1);
            IContact sut = CreateSut(birthday: birthday);

            bool result = sut.HasBirthdayWithinDays(withinDays);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private IContact CreateSut(bool birthdayRegistered = true, DateTime? birthday = null)
        {
            return new Domain.Contacts.Contact(_fixture.BuildPersonNameMock().Object)
            {
                Birthday = birthdayRegistered
                    ? birthday?.Date ?? DateTime.Today.AddYears(_random.Next(25, 50) * -1)
                    : (DateTime?) null
            };
        }
    }
}