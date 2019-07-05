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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers.GetLetterHeadQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetLetterHeadQueryHandler
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

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetLetterHeadQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetLetterHeadQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetLetterHeadQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetLetterHeadQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetLetterHeadAsyncWasCalledOnCommonRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IGetLetterHeadQuery query = CreateQueryMock(number).Object;
            await sut.QueryAsync(query);

            _commonRepositoryMock.Verify(m => m.GetLetterHeadAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsLetterHeadFromCommonRepository()
        {
            ILetterHead letterHead = _fixture.BuildLetterHeadMock().Object;
            QueryHandler sut = CreateSut(letterHead);

            IGetLetterHeadQuery query = CreateQueryMock().Object;
            ILetterHead result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(letterHead));
        }

        private QueryHandler CreateSut(ILetterHead letterHead = null)
        {
            _commonRepositoryMock.Setup(m => m.GetLetterHeadAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => letterHead ?? _fixture.BuildLetterHeadMock().Object));

            return new QueryHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private Mock<IGetLetterHeadQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetLetterHeadQuery> queryMock = new Mock<IGetLetterHeadQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}