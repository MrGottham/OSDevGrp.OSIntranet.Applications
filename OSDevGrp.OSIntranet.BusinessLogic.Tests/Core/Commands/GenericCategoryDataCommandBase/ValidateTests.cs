using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Commands.GenericCategoryDataCommandBase
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
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenHasNecessaryPermissionGetterIsNull_ThrowsArgumentNullException()
        {
	        IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut();

	        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _ => Task.FromResult(new Mock<IGenericCategory>().Object)));

	        // ReSharper disable PossibleNullReferenceException
	        Assert.That(result.ParamName, Is.EqualTo("hasNecessaryPermissionGetter"));
	        // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenGenericCategoryGetterIsNull_ThrowsArgumentNullException()
        {
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("genericCategoryGetter"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithName()
        {
            string name = _fixture.Create<string>();
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(name, value) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Name") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithName()
        {
            string name = _fixture.Create<string>();
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(name, value) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Name") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithName()
        {
            string name = _fixture.Create<string>();
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(name, value) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Name") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            IGenericCategoryDataCommand<IGenericCategory> sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator, () => _fixture.Create<bool>(), _ => Task.FromResult(new Mock<IGenericCategory>().Object));

            Assert.That(result, Is.SameAs(validator));
        }

        private IGenericCategoryDataCommand<IGenericCategory> CreateSut(string name = null)
        {
            return new MyGenericCategoryDataCommand(_fixture.Create<int>(), name ?? _fixture.Create<string>());
        }

        private class MyGenericCategoryDataCommand : BusinessLogic.Core.Commands.GenericCategoryDataCommandBase<IGenericCategory>
        {
            #region Constructor

            public MyGenericCategoryDataCommand(int number, string name)
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