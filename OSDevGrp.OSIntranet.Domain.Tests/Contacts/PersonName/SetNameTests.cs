using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.PersonName
{
    [TestFixture]
    public class SetNameTests
    {
        #region Private variables 

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameIsNull_ThrowsArgumentNullException()
        {
            IPersonName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(null));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameIsEmpty_ThrowsArgumentNullException()
        {
            IPersonName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            IPersonName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(" "));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasOnlyValueForSurname_AssertGivenNameIsSetToNull()
        {
            IPersonName sut = CreateSut();

            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName(surname);

            Assert.That(sut.GivenName, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasOnlyValueForSurname_AssertMiddleNameIsSetToNull()
        {
            IPersonName sut = CreateSut();

            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName(surname);

            Assert.That(sut.MiddleName, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasOnlyValueForSurname_AssertSurnameIsSetToValue()
        {
            IPersonName sut = CreateSut();

            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName(surname);

            Assert.That(sut.Surname, Is.EqualTo(surname));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameAndSurname_AssertGivenNameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {surname}");

            Assert.That(sut.GivenName, Is.EqualTo(givenName));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameAndSurname_AssertMiddleNameIsSetToNull()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {surname}");

            Assert.That(sut.MiddleName, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameAndSurname_AssertSurnameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {surname}");

            Assert.That(sut.Surname, Is.EqualTo(surname));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameOneMiddleNameAndSurname_AssertGivenNameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName} {surname}");

            Assert.That(sut.GivenName, Is.EqualTo(givenName));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameOneMiddleNameAndSurname_AssertMiddleNameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName} {surname}");

            Assert.That(sut.MiddleName, Is.EqualTo(middleName));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameOneMiddleNameAndSurname_AssertSurnameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName} {surname}");

            Assert.That(sut.Surname, Is.EqualTo(surname));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameTwoMiddleNamesAndSurname_AssertGivenNameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName1 = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName2 = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName1} {middleName2} {surname}");

            Assert.That(sut.GivenName, Is.EqualTo(givenName));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameTwoMiddleNamesAndSurname_AssertMiddleNameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName1 = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName2 = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName1} {middleName2} {surname}");

            Assert.That(sut.MiddleName, Is.EqualTo($"{middleName1} {middleName2}"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValueForGivenNameTwoMiddleNamesAndSurname_AssertSurnameIsExtractedAndSet()
        {
            IPersonName sut = CreateSut();

            string givenName = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName1 = _fixture.Create<string>().Replace(" ", string.Empty);
            string middleName2 = _fixture.Create<string>().Replace(" ", string.Empty);
            string surname = _fixture.Create<string>().Replace(" ", string.Empty);
            sut.SetName($"{givenName} {middleName1} {middleName2} {surname}");

            Assert.That(sut.Surname, Is.EqualTo(surname));
        }

        private IPersonName CreateSut()
        {
            return new Domain.Contacts.PersonName(_fixture.Create<string>());
        }
    }
}
