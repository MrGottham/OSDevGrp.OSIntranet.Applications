using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.PersonNameCommand
{
    [TestFixture]
    public class ToDomainTests
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
        public void ToDomain_WhenCommandDoesNotHaveGivenNameAndDoesNotHaveMiddleName_ReturnsPersonName()
        {
            IPersonNameCommand sut = CreateSut(hasGivenName: false, hasMiddleName: false);

            IName result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<PersonName>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithoutGivenName()
        {
            IPersonNameCommand sut = CreateSut(hasGivenName: false, hasMiddleName: false);

            string result = ((IPersonName) sut.ToDomain()).GivenName;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithoutMiddleName()
        {
            IPersonNameCommand sut = CreateSut(hasGivenName: false, hasMiddleName: false);

            string result = ((IPersonName) sut.ToDomain()).MiddleName;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithSurnameFromCommand()
        {
            string surname = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(hasGivenName: false, hasMiddleName: false, surname: surname);

            string result = ((IPersonName) sut.ToDomain()).Surname;

            Assert.That(result, Is.EqualTo(surname));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndDoesNotHaveMiddleName_ReturnsPersonName()
        {
            IPersonNameCommand sut = CreateSut(hasMiddleName: false);

            IName result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<PersonName>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithGivenNameFromCommand()
        {
            string givenName = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(givenName: givenName, hasMiddleName: false);

            string result = ((IPersonName) sut.ToDomain()).GivenName;

            Assert.That(result, Is.EqualTo(givenName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithoutMiddleName()
        {
            IPersonNameCommand sut = CreateSut(hasMiddleName: false);

            string result = ((IPersonName) sut.ToDomain()).MiddleName;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndDoesNotHaveMiddleName_ReturnsPersonNameWithSurnameFromCommand()
        {
            string surname = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(hasMiddleName: false, surname: surname);

            string result = ((IPersonName) sut.ToDomain()).Surname;

            Assert.That(result, Is.EqualTo(surname));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndHasMiddleName_ReturnsPersonName()
        {
            IPersonNameCommand sut = CreateSut();

            IName result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<PersonName>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndHasMiddleName_ReturnsPersonNameWithGivenNameFromCommand()
        {
            string givenName = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(givenName: givenName);

            string result = ((IPersonName) sut.ToDomain()).GivenName;

            Assert.That(result, Is.EqualTo(givenName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndHasMiddleName_ReturnsPersonNameWithMiddleNameFromCommand()
        {
            string middleName = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(middleName: middleName);

            string result = ((IPersonName) sut.ToDomain()).MiddleName;

            Assert.That(result, Is.EqualTo(middleName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasGivenNameAndHasMiddleName_ReturnsPersonNameWithSurnameFromCommand()
        {
            string surname = _fixture.Create<string>();
            IPersonNameCommand sut = CreateSut(surname: surname);

            string result = ((IPersonName) sut.ToDomain()).Surname;

            Assert.That(result, Is.EqualTo(surname));
        }

        private IPersonNameCommand CreateSut(bool hasGivenName = true, string givenName = null, bool hasMiddleName = true, string middleName = null, string surname = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.PersonNameCommand>()
                .With(m => m.GivenName, hasGivenName ? givenName ?? _fixture.Create<string>() : null)
                .With(m => m.MiddleName, hasMiddleName ? middleName ?? _fixture.Create<string>() : null)
                .With(m => m.Surname, surname ?? _fixture.Create<string>())
                .Create();
        }
    }
}