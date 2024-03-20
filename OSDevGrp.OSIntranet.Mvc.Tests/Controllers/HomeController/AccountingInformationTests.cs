using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class AccountingInformationTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingInformation_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.AccountingInformation(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(query =>
                    query != null &&
                    query.AccountingNumber == accountingNumber &&
                    query.StatusDate == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingInformation_WhenNoAccountingWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AccountingInformation(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingInformation_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AccountingInformation(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingInformation_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToAccountingPresentationPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AccountingInformation(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_AccountingPresentationPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingInformation_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingPresentationViewModel()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            Controller sut = CreateSut(accounting: accounting);

            PartialViewResult result = (PartialViewResult) await sut.AccountingInformation(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<AccountingPresentationViewModel>());

            AccountingPresentationViewModel accountingPresentationViewModel = (AccountingPresentationViewModel) result.Model;
            Assert.That(accountingPresentationViewModel, Is.Not.Null);
            Assert.That(accountingPresentationViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool hasAccounting = true, IAccounting accounting = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}