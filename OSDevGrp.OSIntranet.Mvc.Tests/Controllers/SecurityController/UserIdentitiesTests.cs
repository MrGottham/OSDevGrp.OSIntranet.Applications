using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Security;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class UserIdentitiesTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<IUserIdentity>(builder => builder.FromFactory(() => _fixture.BuildUserIdentityMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserIdentities_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.UserIdentities();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IUserIdentity>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserIdentities_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UserIdentities();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserIdentities_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToUserIdentities()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UserIdentities();

            Assert.That(result.ViewName, Is.EqualTo("UserIdentities"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserIdentities_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfUserIdentityViewModel()
        {
            IEnumerable<IUserIdentity> userIdentityMockCollection = _fixture.CreateMany<IUserIdentity>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(userIdentityMockCollection);

            ViewResult result = (ViewResult) await sut.UserIdentities();

            Assert.That(result.Model, Is.TypeOf<List<UserIdentityViewModel>>());

            List<UserIdentityViewModel> userIdentityViewModelCollection = (List<UserIdentityViewModel>) result.Model;

            Assert.That(userIdentityViewModelCollection, Is.Not.Null);
            Assert.That(userIdentityViewModelCollection, Is.Not.Empty);
            Assert.That(userIdentityViewModelCollection.Count, Is.EqualTo(userIdentityMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IUserIdentity> userIdentityCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IUserIdentity>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => userIdentityCollection ?? _fixture.CreateMany<IUserIdentity>(_random.Next(5, 10)).ToList()));

            return new Mvc.Controllers.SecurityController(_queryBusMock.Object);
        }
    }
}
