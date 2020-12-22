using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class PaymentTermsAsyncTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTermsAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.PaymentTermsAsync();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTermsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<PaymentTermModel>> result = await sut.PaymentTermsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTermsAsync_WhenCalled_AssertOkObjectResultContainsPaymentTerms()
        {
            IList<IPaymentTerm> paymentTermMockCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(paymentTermMockCollection);

            OkObjectResult result = (OkObjectResult) (await sut.PaymentTermsAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            IList<PaymentTermModel> paymentTermModels = ((IEnumerable<PaymentTermModel>) result.Value).ToList();
            Assert.That(paymentTermModels, Is.Not.Null);
            Assert.That(paymentTermModels, Is.Not.Empty);
            Assert.That(paymentTermModels.Count, Is.EqualTo(paymentTermMockCollection.Count));
        }

        private Controller CreateSut(IEnumerable<IPaymentTerm> paymentTerms = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(paymentTerms ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}