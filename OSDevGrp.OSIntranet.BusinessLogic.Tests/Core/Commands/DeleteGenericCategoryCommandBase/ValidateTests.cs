using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Commands.DeleteGenericCategoryCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenGenericCategoryGetterIsNull_ThrowsArgumentNullException()
        {
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("genericCategoryGetter"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithNumber()
        {
            int number = _fixture.Create<int>();
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == number),
                    It.Is<Func<int, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Number") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeDeletableWasCalledOnObjectValidatorWithNumber()
        {
            int number = _fixture.Create<int>();
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeDeletable(
                    It.Is<int>(value => value == number),
                    It.Is<Func<int, Task<IGenericCategory>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Number") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            IDeleteGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.SameAs(validator));
        }

        private IDeleteGenericCategoryCommand<IGenericCategory> CreateSut(int? number = null)
        {
            return new MyDeleteGenericCategoryCommand(number ?? _fixture.Create<int>());
        }

        private class MyDeleteGenericCategoryCommand : BusinessLogic.Core.Commands.DeleteGenericCategoryCommandBase<IGenericCategory>
        {
            #region Constructor

            public MyDeleteGenericCategoryCommand(int number)
                : base(number)
            {
            }

            #endregion
        }
    }
}