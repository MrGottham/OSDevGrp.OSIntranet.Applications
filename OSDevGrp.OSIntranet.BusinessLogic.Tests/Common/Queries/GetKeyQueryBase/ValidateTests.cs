using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Queries.GetKeyQueryBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IGetKeyQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithKeyElementCollection()
        {
            IEnumerable<string> keyElementCollection = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IGetKeyQuery sut = CreateSut(keyElementCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<IEnumerable<string>>(value => value != null && value.Equals(keyElementCollection)),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "KeyElementCollection") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldContainItemsWasCalledOnEnumerableValidatorWithKeyElementCollection()
        {
            IEnumerable<string> keyElementCollection = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IGetKeyQuery sut = CreateSut(keyElementCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldContainItems(
                    It.Is<IEnumerable<string>>(value => value != null && value.Equals(keyElementCollection)),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "KeyElementCollection") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IGetKeyQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGetKeyQuery CreateSut(IEnumerable<string> keyElementCollection = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.KeyElementCollection, keyElementCollection ?? _fixture.CreateMany<string>(_random.Next(1,5 )).ToArray())
                .Create();
        }

        private class Sut : BusinessLogic.Common.Queries.GetKeyQueryBase
        {
        }
    }
}