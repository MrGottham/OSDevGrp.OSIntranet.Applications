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
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class PaymentTermsTests
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
        public async Task PaymentTerms_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.PaymentTerms();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTerms_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.PaymentTerms();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTerms_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToPaymentTerms()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.PaymentTerms();

            Assert.That(result.ViewName, Is.EqualTo("PaymentTerms"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PaymentTerms_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfPaymentTermViewModel()
        {
            IEnumerable<IPaymentTerm> paymentTermMockCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(paymentTermMockCollection);

            ViewResult result = (ViewResult) await sut.PaymentTerms();

            Assert.That(result.Model, Is.TypeOf<List<PaymentTermViewModel>>());

            List<PaymentTermViewModel> paymentTermViewModellCollection = (List<PaymentTermViewModel>) result.Model;

            Assert.That(paymentTermViewModellCollection, Is.Not.Null);
            Assert.That(paymentTermViewModellCollection, Is.Not.Empty);
            Assert.That(paymentTermViewModellCollection.Count, Is.EqualTo(paymentTermMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IPaymentTerm> paymentTermCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
