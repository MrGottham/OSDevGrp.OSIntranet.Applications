using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers.PullKeyValueEntryQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.PullKeyValueEntryQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertValidateWasCalledOnPullKeyValueEntryQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IPullKeyValueEntryQuery> pullKeyValueEntryQueryMock = BuildPullKeyValueEntryQueryMock();
            await sut.QueryAsync(pullKeyValueEntryQueryMock.Object);

            pullKeyValueEntryQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertKeyWasCalledOnPullKeyValueEntryQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IPullKeyValueEntryQuery> pullKeyValueEntryQueryMock = BuildPullKeyValueEntryQueryMock();
            await sut.QueryAsync(pullKeyValueEntryQueryMock.Object);

            pullKeyValueEntryQueryMock.Verify(m => m.Key, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertPullKeyValueEntryAsyncWasCalledOnCommonRepository()
        {
            QueryHandler sut = CreateSut();

            string key = _fixture.Create<string>();
            IPullKeyValueEntryQuery pullKeyValueEntryQuery = BuildPullKeyValueEntryQuery(key);
            await sut.QueryAsync(pullKeyValueEntryQuery);

            _commonRepositoryMock.Verify(m => m.PullKeyValueEntryAsync(It.Is<string>(value => string.CompareOrdinal(value, key) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNullAndNoKeyValueEntryWasReturnedFromCommonRepository_ReturnNull()
        {
            QueryHandler sut = CreateSut(false);

            IKeyValueEntry result = await sut.QueryAsync(BuildPullKeyValueEntryQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNullAndKeyValueEntryWasReturnedFromCommonRepository_ReturnNotNull()
        {
            QueryHandler sut = CreateSut();

            IKeyValueEntry result = await sut.QueryAsync(BuildPullKeyValueEntryQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNullAndKeyValueEntryWasReturnedFromCommonRepository_ReturnKeyValueEntryFromCommonRepository()
        {
            IKeyValueEntry keyValueEntry = _fixture.BuildKeyValueEntryMock<object>().Object;
            QueryHandler sut = CreateSut(keyValueEntry: keyValueEntry);

            IKeyValueEntry result = await sut.QueryAsync(BuildPullKeyValueEntryQuery());

            Assert.That(result, Is.EqualTo(keyValueEntry));
        }

        private QueryHandler CreateSut(bool hasKeyValueEntry = true, IKeyValueEntry keyValueEntry = null)
        {
            _commonRepositoryMock.Setup(m => m.PullKeyValueEntryAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(hasKeyValueEntry ? keyValueEntry ?? _fixture.BuildKeyValueEntryMock<object>().Object : null));

            return new QueryHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private IPullKeyValueEntryQuery BuildPullKeyValueEntryQuery(string key = null)
        {
            return BuildPullKeyValueEntryQueryMock(key).Object;
        }

        private Mock<IPullKeyValueEntryQuery> BuildPullKeyValueEntryQueryMock(string key = null)
        {
            Mock<IPullKeyValueEntryQuery> pullKeyValueEntryQueryMock = new Mock<IPullKeyValueEntryQuery>();
            pullKeyValueEntryQueryMock.Setup(m => m.Key)
                .Returns(key ?? _fixture.Create<string>());
            return pullKeyValueEntryQueryMock;
        }
    }
}