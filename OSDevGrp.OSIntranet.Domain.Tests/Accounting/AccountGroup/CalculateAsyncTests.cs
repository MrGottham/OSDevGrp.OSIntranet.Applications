using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroup
{
    [TestFixture]
    public class CalculateAsyncTests
    {
        #region Private variables

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
        public void CalculateAsync_WhenAccountCollectionIsNull_ThrowsArgumentNullException()
        {
            IAccountGroup sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnGivenAccountCollection()
        {
            IAccountGroup sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            await sut.CalculateAsync(statusDate, accountCollectionMock.Object);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsNotNull()
        {
            IAccountGroup sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatus()
        {
            IAccountGroup sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result, Is.TypeOf<Domain.Accounting.AccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereNumberIsEqualToNumberOnAccountGroup()
        {
            int number = _fixture.Create<int>();
            IAccountGroup sut = CreateSut(number);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereNameIsNotNull()
        {
            IAccountGroup sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereNameIsNotEmpty()
        {
            IAccountGroup sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereNameIsEqualToNameOnAccountGroup()
        {
            string name = _fixture.Create<string>();
            IAccountGroup sut = CreateSut(name: name);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(AccountGroupType.Assets)]
        [TestCase(AccountGroupType.Liabilities)]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereAccountGroupTypeIsEqualToAccountGroupTypeOnAccountGroup(AccountGroupType accountGroupType)
        {
            IAccountGroup sut = CreateSut(accountGroupType: accountGroupType);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.AccountGroupType, Is.EqualTo(accountGroupType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereAccountCollectionIsNotNull()
        {
            IAccountGroup sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.AccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereAccountCollectionIsEqualToCalculatedAccountCollectionFromGivenAccountCollection()
        {
            IAccountGroup sut = CreateSut();

            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), accountCollection);

            Assert.That(result.AccountCollection, Is.EqualTo(calculatedAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereStatusDateIsEqualToGivenStatusDate()
        {
            IAccountGroup sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            IAccountGroupStatus result = await sut.CalculateAsync(statusDate, _fixture.BuildAccountCollectionMock().Object);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        private IAccountGroup CreateSut(int? number = null, string name = null, AccountGroupType? accountGroupType = null)
        {
            IAccountGroup accountGroup = new Domain.Accounting.AccountGroup(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>(), accountGroupType ?? _fixture.Create<AccountGroupType>());
            accountGroup.AddAuditInformation(DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>(), DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>());
            return accountGroup;
        }
    }
}