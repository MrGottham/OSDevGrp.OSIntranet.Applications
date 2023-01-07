using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountGroupIdentificationCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _accountingRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _accountingRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertHasNecessaryPermissionWasCalledOnPermissionValidator()
        {
            bool isAccountingAdministrator = _fixture.Create<bool>();
            IAccountGroupIdentificationCommand sut = CreateSut(isAccountingAdministrator);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.PermissionValidatorMock.Verify(m => m.HasNecessaryPermission(It.Is<bool>(value => value == isAccountingAdministrator)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidator()
        {
            int number = _fixture.Create<int>();
            IAccountGroupIdentificationCommand sut = CreateSut(number: number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == number),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Number") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountGroupIdentificationCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountGroupIdentificationCommand CreateSut(bool? isAccountingAdministrator = null, int ? number = null)
        {
            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountGroupIdentificationCommandBase
        {
        }
    }
}