using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.CommandHandlers.ApplyPostingJournalCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.CommandHandlers.ApplyPostingJournalCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Mock<IPostingWarningCalculator> _postingWarningCalculatorMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _postingWarningCalculatorMock = new Mock<IPostingWarningCalculator>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenApplyPostingJournalCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("applyPostingJournalCommand"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnApplyPostingJournalCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IApplyPostingJournalCommand> applyPostingJournalCommandMock = CreateApplyPostingJournalCommandMock();
            await sut.ExecuteAsync(applyPostingJournalCommandMock.Object);

            applyPostingJournalCommandMock.Verify(m => m.Validate(
		            It.Is<IValidator>(value => value == _validatorMock.Object),
		            It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
		            It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object),
		            It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
	            Times.Once);
        }

		[Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnApplyPostingJournalCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IApplyPostingJournalCommand> applyPostingJournalCommandMock = CreateApplyPostingJournalCommandMock();
            await sut.ExecuteAsync(applyPostingJournalCommandMock.Object);

            applyPostingJournalCommandMock.Verify(m => m.ToDomain(It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertApplyPostingJournalAsyncWasCalledOnAccountingRepository()
        {
            CommandHandler sut = CreateSut();

            IPostingJournal postingJournal = _fixture.BuildPostingJournalMock().Object;
            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand(postingJournal);
            await sut.ExecuteAsync(applyPostingJournalCommand);

            _accountingRepositoryMock.Verify(m => m.ApplyPostingJournalAsync(
                    It.Is<IPostingJournal>(value => value == postingJournal),
                    It.Is<IPostingWarningCalculator>(value => value == _postingWarningCalculatorMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_AssertCalculateAsyncWasCalledOnPostingWarningCalculatorWithEmptyPostingLineCollection()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            await sut.ExecuteAsync(applyPostingJournalCommand);

            _postingWarningCalculatorMock.Verify(m => m.CalculateAsync(It.Is<IPostingLineCollection>(value => value != null && value.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsPostingJournalResult()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result, Is.TypeOf<PostingJournalResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsPostingJournalResultWherePostingLineCollectionIsNotNull()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result.PostingLineCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsPostingJournalResultWherePostingLineCollectionIsEmpty()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result.PostingLineCollection, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsPostingJournalResultWherePostingWarningCollectionIsNotNull()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result.PostingWarningCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsPostingJournalResultWherePostingWarningCollectionEqualToPostingWarningCollectionFromPostingWarningCalculator()
        {
            IPostingWarningCollection postingWarningCollection = _fixture.BuildPostingWarningCollectionMock(isEmpty: true).Object;
            CommandHandler sut = CreateSut(false, postingWarningCollection: postingWarningCollection);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result.PostingWarningCollection, Is.EqualTo(postingWarningCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenNoPostingJournalResultWasReturnedFromAccountingRepository_ReturnsCalculatedPostingJournalResult()
        {
            CommandHandler sut = CreateSut(false);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result.StatusDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenPostingJournalResultWasReturnedFromAccountingRepository_AssertCalculateAsyncWasCalledOnPostingJournalResultFromAccountingRepository()
        {
            Mock<IPostingJournalResult> postingJournalResultMock = _fixture.BuildPostingJournalResultMock();
            CommandHandler sut = CreateSut(postingJournalResult: postingJournalResultMock.Object);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            await sut.ExecuteAsync(applyPostingJournalCommand);

            postingJournalResultMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenPostingJournalResultWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            CommandHandler sut = CreateSut();

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenPostingJournalResultWasReturnedFromAccountingRepository_ReturnsCalculatedPostingJournalResult()
        {
            IPostingJournalResult calculatedPostingJournalResult = _fixture.BuildPostingJournalResultMock().Object;
            IPostingJournalResult postingJournalResult = _fixture.BuildPostingJournalResultMock(calculatedPostingJournalResult: calculatedPostingJournalResult).Object;
            CommandHandler sut = CreateSut(postingJournalResult: postingJournalResult);

            IApplyPostingJournalCommand applyPostingJournalCommand = CreateApplyPostingJournalCommand();
            IPostingJournalResult result = await sut.ExecuteAsync(applyPostingJournalCommand);

            Assert.That(result, Is.EqualTo(calculatedPostingJournalResult));
        }

        private CommandHandler CreateSut(bool hasPostingJournalResult = true, IPostingJournalResult postingJournalResult = null, IPostingWarningCollection postingWarningCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.ApplyPostingJournalAsync(It.IsAny<IPostingJournal>(), It.IsAny<IPostingWarningCalculator>()))
                .Returns(Task.FromResult(hasPostingJournalResult ? postingJournalResult ?? _fixture.BuildPostingJournalResultMock().Object : null));

            _postingWarningCalculatorMock.Setup(m => m.CalculateAsync(It.IsAny<IPostingLineCollection>()))
                .Returns(Task.FromResult(postingWarningCollection ?? _fixture.BuildPostingWarningCollectionMock(isEmpty: true).Object));

            return new CommandHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _commonRepositoryMock.Object, _postingWarningCalculatorMock.Object);
        }

        private IApplyPostingJournalCommand CreateApplyPostingJournalCommand(IPostingJournal postingJournal = null)
        {
            return CreateApplyPostingJournalCommandMock(postingJournal).Object;
        }

        private Mock<IApplyPostingJournalCommand> CreateApplyPostingJournalCommandMock(IPostingJournal postingJournal = null)
        {
            Mock<IApplyPostingJournalCommand> applyPostingJournalCommandMock = new Mock<IApplyPostingJournalCommand>();
            applyPostingJournalCommandMock.Setup(m => m.Validate(It.IsAny<IValidator>(), It.IsAny<IClaimResolver>(), It.IsAny<IAccountingRepository>(), It.IsAny<ICommonRepository>()))
                .Returns(_validatorMock.Object);
            applyPostingJournalCommandMock.Setup(m => m.ToDomain(It.IsAny<IAccountingRepository>()))
                .Returns(postingJournal ?? _fixture.BuildPostingJournalMock().Object);
            return applyPostingJournalCommandMock;
        }
    }
}