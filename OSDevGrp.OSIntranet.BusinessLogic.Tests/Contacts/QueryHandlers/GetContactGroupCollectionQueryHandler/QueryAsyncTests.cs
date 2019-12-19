using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetContactGroupCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetContactGroupCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));

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
        public async Task QueryAsync_WhenCalled_AssertGetContactGroupsAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _contactRepositoryMock.Verify(m => m.GetContactGroupsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsContactGroupCollectionFromContactRepository()
        {
            IEnumerable<IContactGroup> contactGroupCollection = _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(contactGroupCollection);

            IEnumerable<IContactGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(contactGroupCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IContactGroup> contactGroupCollection = null)
        {
            _contactRepositoryMock.Setup(m => m.GetContactGroupsAsync())
                .Returns(Task.Run(() => contactGroupCollection ?? _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_contactRepositoryMock.Object);
        }
    }
}
