using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Commands.GenericCategoryIdentificationCommandBase
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
            IGenericCategoryIdentificationCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenGenericCategoryGetterIsNull_ThrowsArgumentNullException()
        {
            IGenericCategoryIdentificationCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("genericCategoryGetter"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithNumber()
        {
            int number = _fixture.Create<int>();
            IGenericCategoryIdentificationCommand<IGenericCategory> sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == number),
                    It.Is<int>(value => value == 1),
                    It.Is<int>(value => value == 99),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Number") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IGenericCategoryIdentificationCommand<IGenericCategory> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            IGenericCategoryIdentificationCommand<IGenericCategory> sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.SameAs(validator));
        }

        private IGenericCategoryIdentificationCommand<IGenericCategory> CreateSut(int? number = null)
        {
            return new MyGenericCategoryIdentificationCommand(number ?? _fixture.Create<int>());
        }

        private class MyGenericCategoryIdentificationCommand : BusinessLogic.Core.Commands.GenericCategoryIdentificationCommandBase<IGenericCategory>
        {
            #region Constructor

            public MyGenericCategoryIdentificationCommand(int number)
                : base(number)
            {
            }

            #endregion
        }
    }
}