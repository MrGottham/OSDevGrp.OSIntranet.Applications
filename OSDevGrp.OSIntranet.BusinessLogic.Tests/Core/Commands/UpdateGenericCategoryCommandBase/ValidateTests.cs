using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Commands.UpdateGenericCategoryCommandBase
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
            IUpdateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenGenericCategoryGetterIsNull_ThrowsArgumentNullException()
        {
            IUpdateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

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
            IUpdateGenericCategoryCommand<IGenericCategory> sut = CreateSut(number);

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
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IUpdateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            IUpdateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator, _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.SameAs(validator));
        }

        private IUpdateGenericCategoryCommand<IGenericCategory> CreateSut(int? number = null)
        {
            return new MyUpdateGenericCategoryCommand(number ?? _fixture.Create<int>(), _fixture.Create<string>());
        }

        private class MyUpdateGenericCategoryCommand : BusinessLogic.Core.Commands.UpdateGenericCategoryCommandBase<IGenericCategory>
        {
            #region Constructor

            public MyUpdateGenericCategoryCommand(int number, string name)
                : base(number, name)
            {
            }

            #endregion

            #region Methods

            public override IGenericCategory ToDomain() => throw new NotSupportedException();

            #endregion
        }
    }
}