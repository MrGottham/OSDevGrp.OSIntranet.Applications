using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactGroupsTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroups_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.ContactGroups();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroups_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ContactGroups();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroups_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToContactGroups()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.ContactGroups();

            Assert.That(result.ViewName, Is.EqualTo("ContactGroups"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactGroups_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfContactGroupViewModel()
        {
            IEnumerable<IContactGroup> contactGroupMockCollection = _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(contactGroupMockCollection);

            ViewResult result = (ViewResult) await sut.ContactGroups();

            Assert.That(result.Model, Is.TypeOf<List<ContactGroupViewModel>>());

            List<ContactGroupViewModel> contactGroupViewModelCollection = (List<ContactGroupViewModel>) result.Model;

            Assert.That(contactGroupViewModelCollection, Is.Not.Null);
            Assert.That(contactGroupViewModelCollection, Is.Not.Empty);
            Assert.That(contactGroupViewModelCollection.Count, Is.EqualTo(contactGroupMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IContactGroup> contactGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => contactGroupCollection ?? _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}