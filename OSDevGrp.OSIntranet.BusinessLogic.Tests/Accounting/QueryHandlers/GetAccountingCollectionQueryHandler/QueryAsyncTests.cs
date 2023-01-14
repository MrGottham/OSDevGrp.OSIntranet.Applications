using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountingCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountingCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingHelper> _accountingHelperMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingHelperMock = new Mock<IAccountingHelper>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountingsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_AssertNumberWasNotCalledOnAnyAccounting()
        {
            Mock<IAccounting>[] accountingMockCollection =
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            QueryHandler sut = CreateSut(false, accountingMockCollection.Select(m => m.Object).ToArray());

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.Number, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_AssertCanAccessAccountingWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.CanAccessAccounting(It.IsAny<int>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_AssertCalculateAsyncWasNotCalledOnAnyAccounting()
        {
            Mock<IAccounting>[] accountingMockCollection =
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            QueryHandler sut = CreateSut(false, accountingMockCollection.Select(m => m.Object).ToArray());

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_AssertApplyLogicForPrincipalWasNotCalledOnAccountingHelper()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(new EmptyQuery());

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<IEnumerable<IAccounting>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingsWasReturnedFromAccountingRepository_ReturnsEmptyAccountingCollection()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_AssertNumberWasCalledOnEachAccounting()
        {
            Mock<IAccounting>[] accountingMockCollection =
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            QueryHandler sut = CreateSut(accountingCollection: accountingMockCollection.Select(m => m.Object).ToArray());

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.Number, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_AssertCanAccessAccountingWasCalledOnClaimResolverWithEachAccountingNumber()
        {
            int[] accountingNumberCollection =
            {
                _fixture.Create<int>(),
                _fixture.Create<int>(),
                _fixture.Create<int>(),
                _fixture.Create<int>(),
                _fixture.Create<int>()
            };
            QueryHandler sut = CreateSut(accountingCollection: accountingNumberCollection.Select(accountingNumber => _fixture.BuildAccountingMock(accountingNumber).Object).ToArray());

            await sut.QueryAsync(new EmptyQuery());

            foreach (int accountingNumber in accountingNumberCollection)
            {
                _claimResolverMock.Verify(m => m.CanAccessAccounting(It.Is<int>(value => value == accountingNumber)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_AssertCalculateAsyncWasCalledOnEachAccessibleAccounting()
        {
            Mock<IAccounting>[] accountingMockCollection =
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            QueryHandler sut = CreateSut(accountingCollection: accountingMockCollection.Select(m => m.Object).ToArray());

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == DateTime.Today)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_AssertCalculateAsyncWasNotCalledOnEachNonAccessibleAccounting()
        {
            Mock<IAccounting>[] accountingMockCollection =
            {
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock(),
                _fixture.BuildAccountingMock()
            };
            QueryHandler sut = CreateSut(accountingCollection: accountingMockCollection.Select(m => m.Object).ToArray(), canAccessAccounting: false);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccounting> accountingMock in accountingMockCollection)
            {
                accountingMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_AssertApplyLogicForPrincipalWasCalledOnAccountingHelperWithAccessibleCalculatedAccountingCollection()
        {
            IEnumerable<IAccounting> calculatedAccountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingCollection: calculatedAccountingCollection.Select(calculatedAccounting => _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object).ToList());

            await sut.QueryAsync(new EmptyQuery());

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<IEnumerable<IAccounting>>(value => value.All(accounting => calculatedAccountingCollection.Any(calculatedAccounting => calculatedAccounting == accounting)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            IEnumerable<IAccounting> calculatedAccountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingCollection: calculatedAccountingCollection.Select(calculatedAccounting => _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object).ToList(), calculatedAccountingCollection: calculatedAccountingCollection);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_ReturnsNonEmptyAccountingCollection()
        {
            IEnumerable<IAccounting> calculatedAccountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingCollection: calculatedAccountingCollection.Select(calculatedAccounting => _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object).ToList(), calculatedAccountingCollection: calculatedAccountingCollection);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingsWasReturnedFromAccountingRepository_ReturnsAccessibleCalculatedAccountingCollectionFromAccountingHelper()
        {
            IEnumerable<IAccounting> calculatedAccountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingCollection: calculatedAccountingCollection.Select(calculatedAccounting => _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object).ToList(), calculatedAccountingCollection: calculatedAccountingCollection);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(calculatedAccountingCollection));
        }

        private QueryHandler CreateSut(bool hasAccountingCollection = true, IEnumerable<IAccounting> accountingCollection = null, IEnumerable<IAccounting> calculatedAccountingCollection = null, bool canAccessAccounting = true)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingsAsync())
                .Returns(Task.FromResult(hasAccountingCollection ? accountingCollection ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList() : null));

            _claimResolverMock.Setup(m => m.CanAccessAccounting(It.IsAny<int>()))
                .Returns(canAccessAccounting);

            _accountingHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<IEnumerable<IAccounting>>()))
                .Returns(calculatedAccountingCollection ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList());

           return new QueryHandler(_accountingRepositoryMock.Object, _claimResolverMock.Object, _accountingHelperMock.Object);
        }
    }
}