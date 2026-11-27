using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactGroupsAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroupsAsync_WhenCalled_QueriesForContactGroups()
        {
            Controller sut = CreateSut();

            await sut.ContactGroupsAsync();

            _queryBusMock.Verify(mock => mock.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroupsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<ContactGroupModel>> result = await sut.ContactGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroupsAsync_WhenCalled_MapsContactGroups()
        {
            IList<IContactGroup> contactGroups =
            [
                _fixture.BuildContactGroupMock(number: 17, name: "Group 17").Object
            ];
            Controller sut = CreateSut(contactGroups);

            OkObjectResult result = (OkObjectResult) (await sut.ContactGroupsAsync()).Result;
            ContactGroupModel model = ((IEnumerable<ContactGroupModel>) result.Value).Single();

            Assert.That(model.Number, Is.EqualTo(17));
            Assert.That(model.Name, Is.EqualTo("Group 17"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroupsAsync_WhenCalled_OrdersContactGroupsByNumber()
        {
            IList<IContactGroup> contactGroups =
            [
                _fixture.BuildContactGroupMock(number: 17).Object,
                _fixture.BuildContactGroupMock(number: 2).Object
            ];
            Controller sut = CreateSut(contactGroups);

            OkObjectResult result = (OkObjectResult) (await sut.ContactGroupsAsync()).Result;

            Assert.That(((IEnumerable<ContactGroupModel>) result.Value).Select(model => model.Number), Is.EqualTo(new[] { 2, 17 }));
        }

        private Controller CreateSut(IEnumerable<IContactGroup> contactGroups = null)
        {
            _queryBusMock.Setup(mock => mock.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(contactGroups ?? Enumerable.Empty<IContactGroup>()));

            return new Controller(_queryBusMock.Object, ConverterFactoryCreator.Create());
        }
    }
}