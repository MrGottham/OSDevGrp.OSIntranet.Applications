using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.DeleteLetterHeadCommand
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
            IDeleteLetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _commonRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IDeleteLetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidator()
        {
            int number = _fixture.Create<int>();
            IDeleteLetterHeadCommand sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == number), It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(value => sut.GetType() == value),
                    It.Is<string>(value => string.Compare(value, "Number", StringComparison.Ordinal) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeDeletableWasCalledOnObjectValidator()
        {
            int number = _fixture.Create<int>();
            IDeleteLetterHeadCommand sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeDeletable(
                    It.Is<int>(value => value == number), It.IsNotNull<Func<int, Task<ILetterHead>>>(),
                    It.Is<Type>(value => sut.GetType() == value),
                    It.Is<string>(value => string.Compare(value, "Number", StringComparison.Ordinal) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IDeleteLetterHeadCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IDeleteLetterHeadCommand CreateSut(int? number = null)
        {
            return _fixture.Build<BusinessLogic.Common.Commands.DeleteLetterHeadCommand>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .Create();
        }
    }
}