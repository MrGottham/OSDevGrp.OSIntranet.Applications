using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.CompanyName
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
            ICompanyName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(null));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameIsEmpty_ThrowsArgumentNullException()
        {
            ICompanyName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            ICompanyName sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SetName(" "));

            Assert.That(result.ParamName, Is.EqualTo("fullName"));
        }

        [Test]
        [Category("UnitTest")]
        public void SetName_WhenFullNameHasValue_AssertFullNameIsSetToValue()
        {
            ICompanyName sut = CreateSut();

            string fullName = _fixture.Create<string>();
            sut.SetName(fullName);

            Assert.That(sut.FullName, Is.EqualTo(fullName));
        }

        private ICompanyName CreateSut()
        {
            return new Domain.Contacts.CompanyName(_fixture.Create<string>());
        }
    }
}
