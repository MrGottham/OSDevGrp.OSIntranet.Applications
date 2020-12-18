using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountDataCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();

            _fixture = new Fixture();
            _fixture.Customize<ICreditInfoCommand>(builder => builder.FromFactory(CreateCreditInfoCommand));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertValidateWasCalledOnEachCreditInfoCommandInCreditInfoCollection()
        {
            Mock<ICreditInfoCommand>[] creditInfoCommandMockCollection =
            {
                CreateCreditInfoCommandMock(),
                CreateCreditInfoCommandMock(),
                CreateCreditInfoCommandMock()
            };
            IAccountDataCommand sut = CreateSut(creditInfoCommandCollection: creditInfoCommandMockCollection.Select(creditInfoCommandMock => creditInfoCommandMock.Object).ToArray());

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            foreach (Mock<ICreditInfoCommand> creditInfoCommandMock in creditInfoCommandMockCollection)
            {
                creditInfoCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once());
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithAccountGroupNumber()
        {
            int accountGroupNumber = _fixture.Create<int>();
            IAccountDataCommand sut = CreateSut(accountGroupNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == accountGroupNumber),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "AccountGroupNumber") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAccountGroupNumber()
        {
            int accountGroupNumber = _fixture.Create<int>();
            IAccountDataCommand sut = CreateSut(accountGroupNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == accountGroupNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "AccountGroupNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithCreditInfoCollection()
        {
            ICreditInfoCommand[] creditInfoCommandCollection = _fixture.CreateMany<ICreditInfoCommand>(_random.Next(5, 10)).ToArray();
            IAccountDataCommand sut = CreateSut(creditInfoCommandCollection: creditInfoCommandCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<IEnumerable<ICreditInfoCommand>>(value => value == creditInfoCommandCollection),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "CreditInfoCollection") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IAccountDataCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IAccountDataCommand CreateSut(int? accountGroupNumber = null, IEnumerable<ICreditInfoCommand> creditInfoCommandCollection = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.AccountGroupNumber, accountGroupNumber ?? _fixture.Create<int>())
                .With(m => m.CreditInfoCollection, creditInfoCommandCollection ?? _fixture.CreateMany<ICreditInfoCommand>(_random.Next(5, 10)).ToArray())
                .Create();
        }

        private ICreditInfoCommand CreateCreditInfoCommand()
        {
            return CreateCreditInfoCommandMock().Object;
        }

        private Mock<ICreditInfoCommand> CreateCreditInfoCommandMock()
        {
            return new Mock<ICreditInfoCommand>();
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountDataCommandBase
        {
        }
    }
}