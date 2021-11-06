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
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ApplyPostingJournalCommand
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
            _fixture.Customize<IApplyPostingLineCommand>(builder => builder.FromFactory(CreateApplyPostingLineCommand));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _accountingRepositoryMock.Object, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _commonRepositoryMock.Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAccountingNumber()
        {
            int accountingNumber = _fixture.Create<int>();
            IApplyPostingJournalCommand sut = CreateSut(accountingNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<int>(value => value == accountingNumber),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "AccountingNumber") == 0),
                    It.Is<bool>(allowNull => allowNull == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithPostingLineCollection()
        {
            IEnumerable<IApplyPostingLineCommand> applyPostingLineCommandCollection = _fixture.CreateMany<IApplyPostingLineCommand>(_random.Next(10, 25)).ToArray();
            IApplyPostingJournalCommand sut = CreateSut(applyPostingLineCommandCollection: applyPostingLineCommandCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<IEnumerable<IApplyPostingLineCommand>>(value => value.Equals(applyPostingLineCommandCollection)),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PostingLineCollection") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldContainItemsWasCalledOnEnumerableValidatorWithPostingLineCollection()
        {
            IEnumerable<IApplyPostingLineCommand> applyPostingLineCommandCollection = _fixture.CreateMany<IApplyPostingLineCommand>(_random.Next(10, 25)).ToArray();
            IApplyPostingJournalCommand sut = CreateSut(applyPostingLineCommandCollection: applyPostingLineCommandCollection);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldContainItems(
                    It.Is<IEnumerable<IApplyPostingLineCommand>>(value => value.Equals(applyPostingLineCommandCollection)),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PostingLineCollection") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            int accountingNumber = _fixture.Create<int>();
            IApplyPostingJournalCommand sut = CreateSut(accountingNumber);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingWasReturnedFromAccountingRepository_AssertValidateWasCalledOnEachApplyPostingLineCommand()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IEnumerable<Mock<IApplyPostingLineCommand>> applyPostingLineCommandMockCollection = new[]
            {
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock()
            };
            IApplyPostingJournalCommand sut = CreateSut(accounting: accounting, applyPostingLineCommandCollection: applyPostingLineCommandMockCollection.Select(applyPostingLineCommandMock => applyPostingLineCommandMock.Object).ToArray());

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            foreach (Mock<IApplyPostingLineCommand> applyPostingLineCommandMock in applyPostingLineCommandMockCollection)
            {
                applyPostingLineCommandMock.Verify(m => m.Validate(
                        It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object),
                        It.Is<IAccounting>(value => value == accounting)),
                    Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAccountingWasNotReturnedFromAccountingRepository_AssertValidateWasNotCalledOnAnyApplyPostingLineCommand()
        {
            IEnumerable<Mock<IApplyPostingLineCommand>> applyPostingLineCommandMockCollection = new[]
            {
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock(),
                CreateApplyPostingLineCommandMock()
            };
            IApplyPostingJournalCommand sut = CreateSut(hasAccounting: false, applyPostingLineCommandCollection: applyPostingLineCommandMockCollection.Select(applyPostingLineCommandMock => applyPostingLineCommandMock.Object).ToArray());

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            foreach (Mock<IApplyPostingLineCommand> applyPostingLineCommandMock in applyPostingLineCommandMockCollection)
            {
                applyPostingLineCommandMock.Verify(m => m.Validate(
                        It.IsAny<IValidator>(),
                        It.IsAny<IAccounting>()),
                    Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IApplyPostingJournalCommand CreateSut(int? accountingNumber = null, bool hasAccounting = true, IAccounting accounting = null, IEnumerable<IApplyPostingLineCommand> applyPostingLineCommandCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));

            return _fixture.Build<BusinessLogic.Accounting.Commands.ApplyPostingJournalCommand>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.PostingLineCollection, applyPostingLineCommandCollection ?? _fixture.CreateMany<IApplyPostingLineCommand>(_random.Next(10, 25)).ToArray())
                .Create();
        }

        private IApplyPostingLineCommand CreateApplyPostingLineCommand()
        {
            return CreateApplyPostingLineCommandMock().Object;
        }

        private Mock<IApplyPostingLineCommand> CreateApplyPostingLineCommandMock()
        {
            Mock<IApplyPostingLineCommand> applyPostingLineCommandMock = new Mock<IApplyPostingLineCommand>();
            applyPostingLineCommandMock.Setup(m => m.Validate(It.IsAny<IValidator>(), It.IsAny<IAccounting>()))
                .Returns(_validatorMockContext.ValidatorMock.Object);
            return applyPostingLineCommandMock;
        }
    }
}