using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroup
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
        public void CalculateAsync_WhenBudgetAccountCollectionIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountGroup sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("budgetAccountCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnGivenBudgetAccountCollection()
        {
            IBudgetAccountGroup sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            await sut.CalculateAsync(statusDate, budgetAccountCollectionMock.Object);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatus()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result, Is.TypeOf<Domain.Accounting.BudgetAccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNumberIsEqualToNumberOnBudgetAccountGroup()
        {
            int number = _fixture.Create<int>();
            IBudgetAccountGroup sut = CreateSut(number);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsNotNull()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsNotEmpty()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereNameIsEqualToNameOnBudgetAccountGroup()
        {
            string name = _fixture.Create<string>();
            IBudgetAccountGroup sut = CreateSut(name: name);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsNotNull()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.BudgetAccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsEqualToCalculatedBudgetAccountCollectionFromGivenBudgetAccountCollection()
        {
            IBudgetAccountGroup sut = CreateSut();

            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(0, 365) * -1), budgetAccountCollection);

            Assert.That(result.BudgetAccountCollection, Is.EqualTo(calculatedBudgetAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereStatusDateIsEqualToGivenStatusDate()
        {
            IBudgetAccountGroup sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 365) * -1);
            IBudgetAccountGroupStatus result = await sut.CalculateAsync(statusDate, _fixture.BuildBudgetAccountCollectionMock().Object);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        private IBudgetAccountGroup CreateSut(int? number = null, string name = null)
        {
            IBudgetAccountGroup budgetAccountGroup = new Domain.Accounting.BudgetAccountGroup(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
            budgetAccountGroup.AddAuditInformation(DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>(), DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>());
            return budgetAccountGroup;
        }
    }
}