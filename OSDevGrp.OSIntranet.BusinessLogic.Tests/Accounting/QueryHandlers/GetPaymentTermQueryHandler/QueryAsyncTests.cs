using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetPaymentTermQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetPaymentTermQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetPaymentTermQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPaymentTermQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetPaymentTermQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPaymentTermQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetPaymentTermAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IGetPaymentTermQuery query = CreateQueryMock(number).Object;
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsPaymentTermFromAccountingRepository()
        {
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            QueryHandler sut = CreateSut(paymentTerm);

            IGetPaymentTermQuery query = CreateQueryMock().Object;
            IPaymentTerm result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(paymentTerm));
        }

        private QueryHandler CreateSut(IPaymentTerm paymentTerm = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => paymentTerm ?? _fixture.BuildPaymentTermMock().Object));

            return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private Mock<IGetPaymentTermQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetPaymentTermQuery> queryMock = new Mock<IGetPaymentTermQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
