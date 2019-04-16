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
    public class ClientSecretIdentitiesTests
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
            _fixture.Customize<IClientSecretIdentity>(builder => builder.FromFactory(() => _fixture.BuildClientSecretIdentityMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ClientSecretIdentities_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.ClientSecretIdentities();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IClientSecretIdentity>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ClientSecretIdentities_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ClientSecretIdentities();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ClientSecretIdentities_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToClientSecretIdentities()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.ClientSecretIdentities();

            Assert.That(result.ViewName, Is.EqualTo("ClientSecretIdentities"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ClientSecretIdentities_WhenCalled_ReturnsViewResultWhereModelIsCollectionOfClientSecretIdentityViewModel()
        {
            IEnumerable<IClientSecretIdentity> clientSecretIdentityMockCollection = _fixture.CreateMany<IClientSecretIdentity>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(clientSecretIdentityMockCollection);

            ViewResult result = (ViewResult) await sut.ClientSecretIdentities();

            Assert.That(result.Model, Is.TypeOf<List<ClientSecretIdentityViewModel>>());

            List<ClientSecretIdentityViewModel> clientSecretIdentityViewModelCollection = (List<ClientSecretIdentityViewModel>) result.Model;

            Assert.That(clientSecretIdentityViewModelCollection, Is.Not.Null);
            Assert.That(clientSecretIdentityViewModelCollection, Is.Not.Empty);
            Assert.That(clientSecretIdentityViewModelCollection.Count, Is.EqualTo(clientSecretIdentityMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<IClientSecretIdentity> clientSecretIdentityCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IClientSecretIdentity>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => clientSecretIdentityCollection ?? _fixture.CreateMany<IClientSecretIdentity>(_random.Next(5, 10)).ToList()));

            return new Mvc.Controllers.SecurityController(_queryBusMock.Object);
        }
    }
}
