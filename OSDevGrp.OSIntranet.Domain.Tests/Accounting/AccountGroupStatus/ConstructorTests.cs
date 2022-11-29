using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
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
        public void Constructor_WhenCalled_AssertNumberWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertNameWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertAccountGroupTypeWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.AccountGroupType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertDeletableWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.Deletable, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertCreatedDateTimeWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.CreatedDateTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertCreatedByIdentifierWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.CreatedByIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertModifiedDateTimeWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.ModifiedDateTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_AssertModifiedByIdentifierWasCalledOnAccountGroup()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            CreateSut(accountGroupMock.Object);

            accountGroupMock.Verify(m => m.ModifiedByIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereNumberIsEqualToNumberFromAccountGroup()
        {
            int number = _fixture.Create<int>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(number).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereNameIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereNameIsNotEmpty()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereNameIsEqualToNameFromAccountGroup()
        {
            string name = _fixture.Create<string>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(name: name).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereAccountGroupTypeIsEqualToAccountGroupTypeFromAccountGroup(AccountGroupType accountGroupType)
        {
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(accountGroupType: accountGroupType).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.AccountGroupType, Is.EqualTo(accountGroupType));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereDeletableIsEqualToDeletableFromAccountGroup(bool deletable)
        {
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(deletable: deletable).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.Deletable, Is.EqualTo(deletable));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereCreatedDateTimeIsEqualToCreatedDateTimeFromAccountGroup()
        {
            DateTime createdDateTime = DateTime.Now.AddMinutes(_random.Next(0, 120) * -1);
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(createdDateTime: createdDateTime).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.CreatedDateTime, Is.EqualTo(createdDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereCreatedByIdentifierIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.CreatedByIdentifier, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereCreatedByIdentifierIsNotEmpty()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.CreatedByIdentifier, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereCreatedByIdentifierIsEqualToCreatedByIdentifierFromAccountGroup()
        {
            string createdByIdentifier = _fixture.Create<string>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(createdByIdentifier: createdByIdentifier).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.CreatedByIdentifier, Is.EqualTo(createdByIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereModifiedDateTimeIsEqualToModifiedDateTimeFromAccountGroup()
        {
            DateTime modifiedDateTime = DateTime.Now.AddMinutes(_random.Next(0, 120) * -1);
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(modifiedDateTime: modifiedDateTime).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.ModifiedDateTime, Is.EqualTo(modifiedDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereModifiedByIdentifierIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ModifiedByIdentifier, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereModifiedByIdentifierIsNotEmpty()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ModifiedByIdentifier, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereModifiedByIdentifierIsEqualToModifiedByIdentifierFromAccountGroup()
        {
            string modifiedByIdentifier = _fixture.Create<string>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(modifiedByIdentifier: modifiedByIdentifier).Object;
            IAccountGroupStatus sut = CreateSut(accountGroup);

            Assert.That(sut.ModifiedByIdentifier, Is.EqualTo(modifiedByIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereAccountCollectionIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.AccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereAccountCollectionIsEqualToAccountCollectionFromArgument()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock().Object;
            IAccountGroupStatus sut = CreateSut(accountCollection: accountCollection);

            Assert.That(sut.AccountCollection, Is.EqualTo(accountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereValuesAtStatusDateIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereValuesAtEndOfLastMonthFromStatusDateIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Constructor_WhenCalled_ReturnsAccountGroupStatusWhereValuesAtEndOfLastYearFromStatusDateIsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.That(sut.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        private IAccountGroupStatus CreateSut(IAccountGroup accountGroup = null, IAccountCollection accountCollection = null)
        {
            return new Domain.Accounting.AccountGroupStatus(accountGroup ?? _fixture.BuildAccountGroupMock().Object, accountCollection ?? _fixture.BuildAccountCollectionMock().Object);
        }
    }
}