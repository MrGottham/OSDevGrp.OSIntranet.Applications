using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.Queries.GenericCategoryIdentificationQueryBase
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
            IGenericCategoryIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithNumber()
        {
            int number = _fixture.Create<int>();
            IGenericCategoryIdentificationQuery sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

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
            IGenericCategoryIdentificationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsSameValidatorAsArgument()
        {
            IGenericCategoryIdentificationQuery sut = CreateSut();

            IValidator validator = _validatorMockContext.ValidatorMock.Object;
            IValidator result = sut.Validate(validator);

            Assert.That(result, Is.SameAs(validator));
        }

        private IGenericCategoryIdentificationQuery CreateSut(int? number = null)
        {
            return new MyGenericCategoryIdentificationQuery(number ?? _fixture.Create<int>());
        }

        private class MyGenericCategoryIdentificationQuery : BusinessLogic.Core.Queries.GenericCategoryIdentificationQueryBase
        {
            #region Constructor

            public MyGenericCategoryIdentificationQuery(int number)
                : base(number)
            {
            }

            #endregion
        }
    }
}