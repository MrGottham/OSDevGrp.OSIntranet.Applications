using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.Contact
{
    [TestFixture]
    public class AgeTests
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
        public void Age_WhenBirthdayIsNull_ReturnsNull()
        {
            IContact sut = CreateSut(false);

            ushort? result = sut.Age;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Age_WhenBirthdayIsNotNull_ReturnsNotNull()
        {
            IContact sut = CreateSut();

            ushort? result = sut.Age;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Age_WhenBirthdayIsBeforeToday_ReturnsCorrectAge()
        {
            ushort numberOfYears = (ushort) _random.Next(10, 75);
            IContact sut = CreateSut(birthday: CreateBirthday(numberOfYears).AddDays(_random.Next(1, 7) * -1));

            ushort? result = sut.Age;

            Assert.That(result, Is.EqualTo(numberOfYears));
        }

        [Test]
        [Category("UnitTest")]
        public void Age_WhenBirthdayIsToday_ReturnsCorrectAge()
        {
            ushort numberOfYears = (ushort) _random.Next(10, 75);
            IContact sut = CreateSut(birthday: CreateBirthday(numberOfYears));

            ushort? result = sut.Age;

            Assert.That(result, Is.EqualTo(numberOfYears));
        }

        [Test]
        [Category("UnitTest")]
        public void Age_WhenBirthdayIsAfterToday_ReturnsCorrectAge()
        {
            ushort numberOfYears = (ushort) _random.Next(10, 75);
            IContact sut = CreateSut(birthday: CreateBirthday(numberOfYears).AddDays(_random.Next(1, 7)));

            ushort? result = sut.Age;

            Assert.That(result, Is.EqualTo(numberOfYears - 1));
        }

        private IContact CreateSut(bool hasBirthday = true, DateTime? birthday = null)
        {
            return new Domain.Contacts.Contact(_fixture.BuildPersonNameMock().Object)
            {
                Birthday = hasBirthday ? birthday ?? CreateBirthday((ushort) _random.Next(10, 75)) : (DateTime?) null
            };
        }

        private DateTime CreateBirthday(ushort numberOfYears)
        {
            return DateTime.Today.AddYears(numberOfYears * -1);
        }
    }
}