using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ApplyPostingJournalCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IApplyPostingLineCommand>(builder => builder.FromFactory(() => CreateApplyPostingLineCommand()));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            int accountingNumber = _fixture.Create<int>();
            IApplyPostingJournalCommand sut = CreateSut(accountingNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertToDomainWasCalledOnEachApplyPostingLineCommand()
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

            sut.ToDomain(_accountingRepositoryMock.Object);

            foreach (Mock<IApplyPostingLineCommand> applyPostingLineCommandMock in applyPostingLineCommandMockCollection)
            {
                applyPostingLineCommandMock.Verify(m => m.ToDomain(It.Is<IAccounting>(value => value == accounting)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournal()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result, Is.TypeOf<PostingJournal>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournalWherePostingLineCollectionNotNull()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result.PostingLineCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournalWherePostingLineCollectionIsPostingLineCollection()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result.PostingLineCollection, Is.TypeOf<PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournalWherePostingLineCollectionNotEmpty()
        {
            IApplyPostingJournalCommand sut = CreateSut();

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result.PostingLineCollection, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournalWherePostingLineCollectionContainingSameAmountOfPostingLinesAsApplyPostingLineCommands()
        {
            int numberOfApplyPostingLineCommands = _random.Next(10, 25);
            IApplyPostingLineCommand[] applyPostingLineCommandCollection = _fixture.CreateMany<IApplyPostingLineCommand>(numberOfApplyPostingLineCommands).ToArray();
            IApplyPostingJournalCommand sut = CreateSut(applyPostingLineCommandCollection: applyPostingLineCommandCollection);

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(result.PostingLineCollection.Count(), Is.EqualTo(numberOfApplyPostingLineCommands));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingJournalWherePostingLineCollectionContainingPostingLinesFromToDomainOnEachApplyPostingLineCommands()
        {
            IPostingLine[] postingLineCollection = new[]
            {
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object,
                _fixture.BuildPostingLineMock().Object
            };
            IApplyPostingJournalCommand sut = CreateSut(applyPostingLineCommandCollection: postingLineCollection.Select(CreateApplyPostingLineCommand).ToArray());

            IPostingJournal result = sut.ToDomain(_accountingRepositoryMock.Object);

            foreach (IPostingLine postingLine in postingLineCollection)
            {
                Assert.That(result.PostingLineCollection.Contains(postingLine), Is.True);
            }
        }

        private IApplyPostingJournalCommand CreateSut(int? accountingNumber = null, IAccounting accounting = null, IEnumerable<IApplyPostingLineCommand> applyPostingLineCommandCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(accounting ?? _fixture.BuildAccountingMock().Object));

            return _fixture.Build<BusinessLogic.Accounting.Commands.ApplyPostingJournalCommand>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.PostingLineCollection, applyPostingLineCommandCollection ?? _fixture.CreateMany<IApplyPostingLineCommand>(_random.Next(10, 25)).ToArray())
                .Create();
        }

        private IApplyPostingLineCommand CreateApplyPostingLineCommand(IPostingLine postingLine = null)
        {
            return CreateApplyPostingLineCommandMock(postingLine).Object;
        }

        private Mock<IApplyPostingLineCommand> CreateApplyPostingLineCommandMock(IPostingLine postingLine = null)
        {
            Mock<IApplyPostingLineCommand> applyPostingLineCommandMock = new Mock<IApplyPostingLineCommand>();
            applyPostingLineCommandMock.Setup(m => m.ToDomain(It.IsAny<IAccounting>()))
                .Returns(postingLine ?? _fixture.BuildPostingLineMock().Object);
            return applyPostingLineCommandMock;
        }
    }
}