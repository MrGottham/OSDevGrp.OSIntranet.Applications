using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.GetContactWithBirthdayCollectionQuery
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
            IGetContactWithBirthdayCollectionQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanOrEqualToZeroWasCalledOnIntegerValidatorForBirthdayWithinDays()
        {
            int birthdayWithinDays = _fixture.Create<int>();
            IGetContactWithBirthdayCollectionQuery sut = CreateSut(birthdayWithinDays);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);
            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeGreaterThanOrEqualToZero(
                    It.Is<int>(value => value == birthdayWithinDays),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "BirthdayWithinDays") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IGetContactWithBirthdayCollectionQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGetContactWithBirthdayCollectionQuery CreateSut(int? birthdayWithinDays = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Queries.GetContactWithBirthdayCollectionQuery>()
                .With(m => m.BirthdayWithinDays, birthdayWithinDays ?? _fixture.Create<int>())
                .Create();
        }
    }
}