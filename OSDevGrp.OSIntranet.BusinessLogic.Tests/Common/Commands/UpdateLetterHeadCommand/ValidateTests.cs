using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.UpdateLetterHeadCommand
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
            IUpdateLetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _commonRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateLetterHeadCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidator()
        {
            int number = _fixture.Create<int>();
            IUpdateLetterHeadCommand sut = CreateSut(number);

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
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IUpdateLetterHeadCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IUpdateLetterHeadCommand CreateSut(int? number = null)
        {
            return _fixture.Build<BusinessLogic.Common.Commands.UpdateLetterHeadCommand>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .Create();
        }
    }
}