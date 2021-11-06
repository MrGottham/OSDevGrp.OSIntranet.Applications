using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.ExternalData.Queries.GetNewsCollectionQuery
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
            IGetNewsCollectionQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithNumberOfNews()
        {
            int numberOfNews = _fixture.Create<int>();
            IGetNewsCollectionQuery sut = CreateSut(numberOfNews);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == numberOfNews),
                    It.Is<int>(minValue => minValue == 0),
                    It.Is<int>(maxValue => maxValue == 250),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "NumberOfNews") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IGetNewsCollectionQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGetNewsCollectionQuery CreateSut(int? numberOfNews = null)
        {
            return new OSDevGrp.OSIntranet.BusinessLogic.ExternalData.Queries.GetNewsCollectionQuery(_fixture.Create<bool>(), numberOfNews ?? _fixture.Create<int>());
        }
    }
}