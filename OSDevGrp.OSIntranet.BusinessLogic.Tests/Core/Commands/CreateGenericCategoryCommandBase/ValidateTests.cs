using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Commands.CreateGenericCategoryCommandBase
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
            ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenHasNecessaryPermissionGetterIsNull_ThrowsArgumentNullException()
        {
	        ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

	        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

	        // ReSharper disable PossibleNullReferenceException
	        Assert.That(result.ParamName, Is.EqualTo("hasNecessaryPermissionGetter"));
	        // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenGenericCategoryGetterIsNull_ThrowsArgumentNullException()
        {
            ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("genericCategoryGetter"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeUnknownValueWasCalledOnObjectValidatorWithNumber()
        {
            int number = _fixture.Create<int>();
            ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
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
            ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            ICreateGenericCategoryCommand<IGenericCategory> sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.SameAs(validator));
        }

        private ICreateGenericCategoryCommand<IGenericCategory> CreateSut(int? number = null)
        {
            return new MyCreateGenericCategoryCommand(number ?? _fixture.Create<int>(), _fixture.Create<string>());
        }

        private class MyCreateGenericCategoryCommand : BusinessLogic.Core.Commands.CreateGenericCategoryCommandBase<IGenericCategory>
        {
            #region Constructor

            public MyCreateGenericCategoryCommand(int number, string name)
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