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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountCoreDataCommandBase
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
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
        {
	        IAccountCoreDataCommand<IAccountBase> sut = CreateSut();

	        ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

	        // ReSharper disable PossibleNullReferenceException
	        Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
	        // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAccountName()
        {
            string accountName = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(accountName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => value == accountName),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountName") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithAccountName()
        {
            string accountName = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(accountName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == accountName),
                    It.Is<int>(minLength => minLength == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountName") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithAccountName()
        {
            string accountName = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(accountName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == accountName),
                    It.Is<int>(maxLength => maxLength == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountName") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
        {
            string description = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(description: description);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == description),
                    It.Is<int>(minLength => minLength == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Description") == 0),
                    It.Is<bool>(allowNull => allowNull)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
        {
            string description = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(description: description);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == description),
                    It.Is<int>(maxLength => maxLength == 512),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Description") == 0),
                    It.Is<bool>(allowNull => allowNull)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithNote()
        {
            string note = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(note: note);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => value == note),
                    It.Is<int>(minLength => minLength == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Note") == 0),
                    It.Is<bool>(allowNull => allowNull)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithNote()
        {
            string note = _fixture.Create<string>();
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut(note: note);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => value == note),
                    It.Is<int>(maxLength => maxLength == 4096),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Note") == 0),
                    It.Is<bool>(allowNull => allowNull)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountCoreDataCommand<IAccountBase> sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountCoreDataCommand<IAccountBase> CreateSut(string accountName = null, string description = null, string note = null)
        {
	        _claimResolverMock.Setup(m => m.IsAccountingCreator())
		        .Returns(_fixture.Create<bool>());
	        _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
		        .Returns(_fixture.Create<bool>());

            return _fixture.Build<Sut>()
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, description ?? _fixture.Create<string>())
                .With(m => m.Note, note ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountCoreDataCommandBase<IAccountBase>
        {
            #region Methods

            public override IAccountBase ToDomain(IAccountingRepository accountingRepository) => throw new NotSupportedException();

            #endregion
        }
    }
}