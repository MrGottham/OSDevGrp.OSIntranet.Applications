using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.UpdateAccountingCommand
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
            IUpdateAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
        {
	        IUpdateAccountingCommand sut = CreateSut();

	        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

	        // ReSharper disable PossibleNullReferenceException
	        Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
	        // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUpdateAccountingCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidator()
        {
            int accountingNumber = _fixture.Create<int>();
            IUpdateAccountingCommand sut = CreateSut(accountingNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == accountingNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountingNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IUpdateAccountingCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IUpdateAccountingCommand CreateSut(int? accountingNumber = null)
        {
	        _claimResolverMock.Setup(m => m.IsAccountingCreator())
		        .Returns(_fixture.Create<bool>());
	        _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
		        .Returns(_fixture.Create<bool>());

            return _fixture.Build<BusinessLogic.Accounting.Commands.UpdateAccountingCommand>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .Create();
        }
   }
}