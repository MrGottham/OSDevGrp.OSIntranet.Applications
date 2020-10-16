using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Queries.AccountIdentificationQueryBase
{
    [TestFixture]
    public class AccountNumberTests
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
        public void AccountNumber_WhenSetValueIsNull_ThrowsArgumentNullException()
        {
            IAccountIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = null);

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsEmpty_ThrowsArgumentNullException()
        {
            IAccountIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = string.Empty);

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = " ");

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsNullEmptyOrWhiteSpace_AssertAccountNumberIsConvertedToUpperCase()
        {
            IAccountIdentificationQuery sut = CreateSut();

            string accountNumber = _fixture.Create<string>().ToLower();
            sut.AccountNumber = accountNumber;

            string result = sut.AccountNumber;

            Assert.That(result, Is.EqualTo(accountNumber.ToUpper()));
        }

        private IAccountIdentificationQuery CreateSut()
        {
            return _fixture.Create<Sut>();
        }

        private class Sut : BusinessLogic.Accounting.Queries.AccountIdentificationQueryBase
        {
        }
    }
}