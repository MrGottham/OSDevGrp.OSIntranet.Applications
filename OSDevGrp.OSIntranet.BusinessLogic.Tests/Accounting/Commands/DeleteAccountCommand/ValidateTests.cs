using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.DeleteAccountCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IDeleteAccountCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
        {
	        IDeleteAccountCommand sut = CreateSut();

	        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

	        // ReSharper disable PossibleNullReferenceException
	        Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
	        // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IDeleteAccountCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IDeleteAccountCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IDeleteAccountCommand sut = CreateSut(accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeDeletableWasCalledOnObjectValidatorWithAccountNumber()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IDeleteAccountCommand sut = CreateSut(accountNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeDeletable(
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.IsNotNull<Func<string, Task<IAccount>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IDeleteAccountCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IDeleteAccountCommand CreateSut(string accountNumber = null)
        {
	        _claimResolverMock.Setup(m => m.IsAccountingCreator())
		        .Returns(_fixture.Create<bool>());
	        _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
		        .Returns(_fixture.Create<bool>());

            return _fixture.Build<BusinessLogic.Accounting.Commands.DeleteAccountCommand>()
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .Create();
        }
    }
}