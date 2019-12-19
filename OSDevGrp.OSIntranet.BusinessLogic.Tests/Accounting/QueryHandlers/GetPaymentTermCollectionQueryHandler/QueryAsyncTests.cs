using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetPaymentTermCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetPaymentTermCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));

            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetPaymentTermsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsPaymentTermCollectionFromAccountingRepository()
        {
            IEnumerable<IPaymentTerm> paymentTermCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(paymentTermCollection);

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(paymentTermCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IPaymentTerm> paymentTermCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermsAsync())
                .Returns(Task.Run(() => paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_accountingRepositoryMock.Object);
        }
    }
}
