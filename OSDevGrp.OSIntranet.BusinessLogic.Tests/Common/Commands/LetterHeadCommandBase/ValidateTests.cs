using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.LetterHeadCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            ILetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _commonRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            ILetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Name", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForLine1()
        {
            string line1 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line1: line1);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, line1) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line1", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine1()
        {
            string line1 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line1: line1);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line1) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line1", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine1()
        {
            string line1 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line1: line1);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line1) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line1", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine2()
        {
            string line2 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line2: line2);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line2) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line2", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine2()
        {
            string line2 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line2: line2);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line2) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line2", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine3()
        {
            string line3 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line3: line3);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line3) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line3", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine3()
        {
            string line3 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line3: line3);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line3) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line3", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine4()
        {
            string line4 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line4: line4);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line4) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line4", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine4()
        {
            string line4 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line4: line4);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line4) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line4", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine5()
        {
            string line5 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line5: line5);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line5) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line5", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine5()
        {
            string line5 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line5: line5);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line5) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line5", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine6()
        {
            string line6 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line6: line6);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line6) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line6", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine6()
        {
            string line6 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line6: line6);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line6) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line6", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForLine7()
        {
            string line7 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line7: line7);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line7) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line7", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForLine7()
        {
            string line7 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line7: line7);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, line7) == 0),
                    It.Is<int>(value => value == 64),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "Line7", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForCompanyIdentificationNumber()
        {
            string companyIdentificationNumber = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(companyIdentificationNumber: companyIdentificationNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, companyIdentificationNumber) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "CompanyIdentificationNumber", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForCompanyIdentificationNumber()
        {
            string companyIdentificationNumber = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(companyIdentificationNumber: companyIdentificationNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, companyIdentificationNumber) == 0),
                    It.Is<int>(value => value == 32),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "CompanyIdentificationNumber", false) == 0),
                    It.Is<bool>(value => value == true)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            ILetterHeadCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private ILetterHeadCommand CreateSut(string name = null, string line1 = null, string line2 = null, string line3 = null, string line4 = null, string line5 = null, string line6 = null, string line7 = null, string companyIdentificationNumber = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.Line1, line1 ?? _fixture.Create<string>())
                .With(m => m.Line2, line2 ?? _fixture.Create<string>())
                .With(m => m.Line3, line3 ?? _fixture.Create<string>())
                .With(m => m.Line4, line4 ?? _fixture.Create<string>())
                .With(m => m.Line5, line5 ?? _fixture.Create<string>())
                .With(m => m.Line6, line6 ?? _fixture.Create<string>())
                .With(m => m.Line7, line7 ?? _fixture.Create<string>())
                .With(m => m.CompanyIdentificationNumber, companyIdentificationNumber ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Common.Commands.LetterHeadCommandBase
        {
        }
    }
}