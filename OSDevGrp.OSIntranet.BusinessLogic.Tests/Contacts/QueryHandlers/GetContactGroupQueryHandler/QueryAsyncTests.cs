using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetContactGroupQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetContactGroupQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetContactGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetContactGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetContactGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetContactGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactGroupAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IGetContactGroupQuery query = CreateQueryMock(number).Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.GetContactGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsContactGroupFromContactRepository()
        {
            IContactGroup contactGroup = _fixture.BuildContactGroupMock().Object;
            QueryHandler sut = CreateSut(contactGroup);

            IGetContactGroupQuery query = CreateQueryMock().Object;
            IContactGroup result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(contactGroup));
        }

        private QueryHandler CreateSut(IContactGroup contactGroup = null)
        {
            _contactRepositoryMock.Setup(m => m.GetContactGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => contactGroup ?? _fixture.BuildContactGroupMock().Object));

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetContactGroupQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetContactGroupQuery> queryMock = new Mock<IGetContactGroupQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
