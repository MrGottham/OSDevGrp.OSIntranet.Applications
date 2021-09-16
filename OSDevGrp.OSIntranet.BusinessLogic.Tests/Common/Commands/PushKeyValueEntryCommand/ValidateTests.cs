using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.PushKeyValueEntryCommand
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
            IPushKeyValueEntryCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IPushKeyValueEntryCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithValue()
        {
            object obj = _fixture.Create<object>();
            IPushKeyValueEntryCommand sut = CreateSut(obj);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<object>(value => value == obj),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Value") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IPushKeyValueEntryCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IPushKeyValueEntryCommand CreateSut(object value = null)
        {
            return _fixture.Build<BusinessLogic.Common.Commands.PushKeyValueEntryCommand>()
                .With(m => m.Key, _fixture.Create<string>())
                .With(m => m.Value, value ?? _fixture.Create<object>())
                .Create();
        }
    }
}