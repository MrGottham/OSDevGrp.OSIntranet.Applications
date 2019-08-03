using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountingQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountingQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetAccountingQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountingQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetAccountingQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountingQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            IGetAccountingQuery query = CreateQueryMock(accountingNumber).Object;
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(It.Is<int>(value => value == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsAccountingFromAccountingRepository()
        {
            IAccounting accounting = _fixture.Create<IAccounting>();
            QueryHandler sut = CreateSut(accounting);

            IGetAccountingQuery query = CreateQueryMock().Object;
            IAccounting result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(accounting));
        }

        private QueryHandler CreateSut(IAccounting accounting = null)
        {
             _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => accounting ?? _fixture.Create<IAccounting>()));

           return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IGetAccountingQuery> CreateQueryMock(int? accountingNumber = null)
        {
            Mock<IGetAccountingQuery> queryMock = new Mock<IGetAccountingQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}