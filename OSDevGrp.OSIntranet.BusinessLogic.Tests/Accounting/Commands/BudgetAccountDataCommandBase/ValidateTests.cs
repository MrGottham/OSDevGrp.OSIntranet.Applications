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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.BudgetAccountDataCommandBase
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
            _fixture.Customize<IBudgetInfoCommand>(builder => builder.FromFactory(CreateBudgetInfoCommand));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _commonRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertValidateWasCalledOnEachBudgetInfoCommandInBudgetInfoCollection()
        {
            Mock<IBudgetInfoCommand>[] budgetInfoCommandMockCollection =
            {
                CreateBudgetInfoCommandMock(),
                CreateBudgetInfoCommandMock(),
                CreateBudgetInfoCommandMock()
            };
            IBudgetAccountDataCommand sut = CreateSut(budgetInfoCommandCollection: budgetInfoCommandMockCollection.Select(budgetInfoCommandMock => budgetInfoCommandMock.Object).ToArray());

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            foreach (Mock<IBudgetInfoCommand> budgetInfoCommandMock in budgetInfoCommandMockCollection)
            {
                budgetInfoCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithBudgetAccountGroupNumber()
        {
            int budgetAccountGroupNumber = _fixture.Create<int>();
            IBudgetAccountDataCommand sut = CreateSut(budgetAccountGroupNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
                    It.Is<int>(value => value == budgetAccountGroupNumber),
                    It.Is<int>(minValue => minValue == 1),
                    It.Is<int>(maxValue => maxValue == 99),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "BudgetAccountGroupNumber") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBudgetAccountGroupNumber()
        {
            int budgetAccountGroupNumber = _fixture.Create<int>();
            IBudgetAccountDataCommand sut = CreateSut(budgetAccountGroupNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == budgetAccountGroupNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => String.CompareOrdinal(field, "BudgetAccountGroupNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithBudgetInfoCollection()
        {
            IBudgetInfoCommand[] budgetInfoCommandCollection = _fixture.CreateMany<IBudgetInfoCommand>(_random.Next(5, 10)).ToArray();
            IBudgetAccountDataCommand sut = CreateSut(budgetInfoCommandCollection: budgetInfoCommandCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<IEnumerable<IBudgetInfoCommand>>(value => value == budgetInfoCommandCollection),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "BudgetInfoCollection") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
        }

        private IBudgetAccountDataCommand CreateSut(int? budgetAccountGroupNumber = null, IEnumerable<IBudgetInfoCommand> budgetInfoCommandCollection = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.BudgetAccountGroupNumber, budgetAccountGroupNumber ?? _fixture.Create<int>())
                .With(m => m.BudgetInfoCollection, budgetInfoCommandCollection ?? _fixture.CreateMany<IBudgetInfoCommand>(_random.Next(5, 10)).ToArray())
                .Create();
        }

        private IBudgetInfoCommand CreateBudgetInfoCommand()
        {
            return CreateBudgetInfoCommandMock().Object;
        }

        private Mock<IBudgetInfoCommand> CreateBudgetInfoCommandMock()
        {
            return new Mock<IBudgetInfoCommand>();
        }

        private class Sut : BusinessLogic.Accounting.Commands.BudgetAccountDataCommandBase
        {
        }
    }
}