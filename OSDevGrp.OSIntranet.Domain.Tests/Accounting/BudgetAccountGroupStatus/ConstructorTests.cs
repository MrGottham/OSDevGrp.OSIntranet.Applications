using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class ConstructorTests
    {
        #region Properties

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertNumberWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertNameWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertDeletableWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.Deletable, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertCreatedDateTimeWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.CreatedDateTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertCreatedByIdentifierWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.CreatedByIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertModifiedDateTimeWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.ModifiedDateTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertModifiedByIdentifierWasCalledOnBudgetAccountGroup()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            CreateSut(budgetAccountGroupMock.Object);

            budgetAccountGroupMock.Verify(m => m.ModifiedByIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNumberIsEqualToNumberFromBudgetAccountGroup()
        {
            int number = _fixture.Create<int>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(number).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsNotEmpty()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsEqualToNameFromBudgetAccountGroup()
        {
            string name = _fixture.Create<string>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(name: name).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereDeletableIsEqualToDeletableFromBudgetAccountGroup(bool deletable)
        {
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(deletable: deletable).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.Deletable, Is.EqualTo(deletable));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereCreatedDateTimeIsEqualToCreatedDateTimeFromBudgetAccountGroup()
        {
            DateTime createdDateTime = DateTime.Now.AddMinutes(_random.Next(0, 120) * -1);
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(createdDateTime: createdDateTime).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.CreatedDateTime, Is.EqualTo(createdDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereCreatedByIdentifierIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.CreatedByIdentifier, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereCreatedByIdentifierIsNotEmpty()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.CreatedByIdentifier, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereCreatedByIdentifierIsEqualToCreatedByIdentifierFromBudgetAccountGroup()
        {
            string createdByIdentifier = _fixture.Create<string>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(createdByIdentifier: createdByIdentifier).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.CreatedByIdentifier, Is.EqualTo(createdByIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereModifiedDateTimeIsEqualToModifiedDateTimeFromBudgetAccountGroup()
        {
            DateTime modifiedDateTime = DateTime.Now.AddMinutes(_random.Next(0, 120) * -1);
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(modifiedDateTime: modifiedDateTime).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.ModifiedDateTime, Is.EqualTo(modifiedDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereModifiedByIdentifierIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ModifiedByIdentifier, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereModifiedByIdentifierIsNotEmpty()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ModifiedByIdentifier, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereModifiedByIdentifierIsEqualToModifiedByIdentifierFromBudgetAccountGroup()
        {
            string modifiedByIdentifier = _fixture.Create<string>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(modifiedByIdentifier: modifiedByIdentifier).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountGroup);

            Assert.That(sut.ModifiedByIdentifier, Is.EqualTo(modifiedByIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.BudgetAccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsEqualToBudgetAccountCollectionFromArgument()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock().Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            Assert.That(sut.BudgetAccountCollection, Is.EqualTo(budgetAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereValuesForMonthOfStatusDateIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesForMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereValuesForLastMonthOfStatusDateIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesForLastMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereValuesForYearToDateOfStatusDateIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesForYearToDateOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsBudgetAccountGroupStatusWhereValuesForLastYearOfStatusDateIsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesForLastYearOfStatusDate, Is.Not.Null);
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountGroup budgetAccountGroup = null, IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(budgetAccountGroup ?? _fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}