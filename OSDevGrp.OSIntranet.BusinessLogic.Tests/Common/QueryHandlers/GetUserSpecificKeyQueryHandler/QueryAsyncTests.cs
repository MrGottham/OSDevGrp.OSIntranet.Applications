using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers.GetUserSpecificKeyQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetUserSpecificKeyQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IKeyGenerator> _keyGeneratorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _keyGeneratorMock = new Mock<IKeyGenerator>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenQueryIsNotNull_AssertValidateWasCalledOnGetUserSpecificKeyQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetUserSpecificKeyQuery> getUserSpecificKeyQueryMock = BuildGetUserSpecificKeyQueryMock();
            await sut.QueryAsync(getUserSpecificKeyQueryMock.Object);

            getUserSpecificKeyQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertKeyElementCollectionWasCalledOnGetUserSpecificKeyQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetUserSpecificKeyQuery> getUserSpecificKeyQueryMock = BuildGetUserSpecificKeyQueryMock();
            await sut.QueryAsync(getUserSpecificKeyQueryMock.Object);

            getUserSpecificKeyQueryMock.Verify(m => m.KeyElementCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertGenerateUserSpecificKeyAsyncWasCalledOnKeyGenerator()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<string> keyElementCollection = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IGetUserSpecificKeyQuery getUserSpecificKeyQuery = BuildGetUserSpecificKeyQuery(keyElementCollection);
            await sut.QueryAsync(getUserSpecificKeyQuery);

            _keyGeneratorMock.Verify(m => m.GenerateUserSpecificKeyAsync(It.Is<IEnumerable<string>>(value => value != null && Equals(value, keyElementCollection))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertGenerateGenericKeyAsyncWasNotCalledOnKeyGenerator()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(BuildGetUserSpecificKeyQuery());

            _keyGeneratorMock.Verify(m => m.GenerateGenericKeyAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            string result = await sut.QueryAsync(BuildGetUserSpecificKeyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsNonEmptyKey()
        {
            QueryHandler sut = CreateSut();

            string result = await sut.QueryAsync(BuildGetUserSpecificKeyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsKeyFromKeyGenerator()
        {
            string generatedKey = _fixture.Create<string>();
            QueryHandler sut = CreateSut(generatedKey);

            string result = await sut.QueryAsync(BuildGetUserSpecificKeyQuery());

            Assert.That(result, Is.EqualTo(generatedKey));
        }

        private QueryHandler CreateSut(string generatedKey = null)
        {
            _keyGeneratorMock.Setup(m => m.GenerateUserSpecificKeyAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(generatedKey ?? _fixture.Create<string>()));

            return new QueryHandler(_validatorMock.Object, _keyGeneratorMock.Object);
        }

        private IGetUserSpecificKeyQuery BuildGetUserSpecificKeyQuery(IEnumerable<string> keyElementCollection = null)
        {
            return BuildGetUserSpecificKeyQueryMock(keyElementCollection).Object;
        }

        private Mock<IGetUserSpecificKeyQuery> BuildGetUserSpecificKeyQueryMock(IEnumerable<string> keyElementCollection = null)
        {
            Mock<IGetUserSpecificKeyQuery> getUserSpecificKeyQueryMock = new Mock<IGetUserSpecificKeyQuery>();
            getUserSpecificKeyQueryMock.Setup(m => m.KeyElementCollection)
                .Returns(keyElementCollection ?? _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray());
            return getUserSpecificKeyQueryMock;
        }
    }
}