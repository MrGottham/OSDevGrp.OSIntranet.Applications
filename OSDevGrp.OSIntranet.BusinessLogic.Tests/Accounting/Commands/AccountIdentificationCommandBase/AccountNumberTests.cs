using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountIdentificationCommandBase
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
            IAccountIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = null);

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsEmpty_ThrowsArgumentNullException()
        {
            IAccountIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = string.Empty);

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AccountNumber = " ");

            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountNumber_WhenSetValueIsNullEmptyOrWhiteSpace_AssertAccountNumberIsConvertedToUpperCase()
        {
            IAccountIdentificationCommand sut = CreateSut();

            string accountNumber = _fixture.Create<string>().ToLower();
            sut.AccountNumber = accountNumber;

            string result = sut.AccountNumber;

            Assert.That(result, Is.EqualTo(accountNumber.ToUpper()));
        }

        private IAccountIdentificationCommand CreateSut()
        {
            return _fixture.Create<Sut>();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountIdentificationCommandBase
        {
        }
    }
}