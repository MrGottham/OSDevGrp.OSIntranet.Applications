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
using OSDevGrp.OSIntranet.Core;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Common.QueryHandlers.GetKeyQueryHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries.IGetKeyQuery>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetKeyQueryHandlerBase
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
        public async Task QueryAsync_WhenQueryIsNotNull_AssertValidateWasCalledOnGetKeyQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetKeyQuery> getKeyQueryMock = BuildGetKeyQueryMock();
            await sut.QueryAsync(getKeyQueryMock.Object);

            getKeyQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertKeyElementCollectionWasCalledOnGetKeyQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetKeyQuery> getKeyQueryMock = BuildGetKeyQueryMock();
            await sut.QueryAsync(getKeyQueryMock.Object);

            getKeyQueryMock.Verify(m => m.KeyElementCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertGenerateKeyWasCalledOnSut()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(BuildGetKeyQuery());

            Assert.That(((Sut)sut).GenerateKeyHasBeenCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertGenerateKeyWasCalledOnSutWithKeyGenerator()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(BuildGetKeyQuery());

            Assert.That(((Sut)sut).GenerateKeyHasBeenCalledKeyGenerator, Is.EqualTo(_keyGeneratorMock.Object));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_AssertGenerateKeyWasCalledOnSutWithKeyElementCollectionFromGetKeyQuery()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<string> keyElementCollection = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IGetKeyQuery getKeyQuery = BuildGetKeyQuery(keyElementCollection);
            await sut.QueryAsync(getKeyQuery);

            Assert.That(((Sut)sut).GenerateKeyHasBeenCalledKeyElementCollection, Is.EqualTo(keyElementCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            string result = await sut.QueryAsync(BuildGetKeyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsNonEmptyKey()
        {
            QueryHandler sut = CreateSut();

            string result = await sut.QueryAsync(BuildGetKeyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenQueryIsNotNull_ReturnsKeyFromGenerateKey()
        {
            string key = _fixture.Create<string>();
            QueryHandler sut = CreateSut(key);

            string result = await sut.QueryAsync(BuildGetKeyQuery());

            Assert.That(result, Is.EqualTo(key));
        }

        private QueryHandler CreateSut(string key = null)
        {
            return new Sut(_validatorMock.Object, _keyGeneratorMock.Object, key ?? _fixture.Create<string>());
        }

        private IGetKeyQuery BuildGetKeyQuery(IEnumerable<string> keyElementCollection = null)
        {
            return BuildGetKeyQueryMock(keyElementCollection).Object;
        }

        private Mock<IGetKeyQuery> BuildGetKeyQueryMock(IEnumerable<string> keyElementCollection = null)
        {
            Mock<IGetKeyQuery> getKeyQueryMock = new Mock<IGetKeyQuery>();
            getKeyQueryMock.Setup(m => m.KeyElementCollection)
                .Returns(keyElementCollection ?? _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray());
            return getKeyQueryMock;
        }

        private class Sut : QueryHandler
        {
            #region Private variables

            private readonly string _key;

            #endregion

            #region Constructor

            public Sut(IValidator validator, IKeyGenerator keyGenerator, string key) 
                : base(validator, keyGenerator)
            {
                NullGuard.NotNullOrWhiteSpace(key, nameof(key));

                _key = key;
            }

            #endregion

            #region Properties

            public bool GenerateKeyHasBeenCalled { get; private set; }

            public IKeyGenerator GenerateKeyHasBeenCalledKeyGenerator { get; private set; }

            public IEnumerable<string> GenerateKeyHasBeenCalledKeyElementCollection { get; private set; }

            #endregion

            #region Methods

            protected override Task<string> GenerateKey(IKeyGenerator keyGenerator, IEnumerable<string> keyElementCollection)
            {
                NullGuard.NotNull(keyGenerator, nameof(keyGenerator))
                    .NotNull(keyElementCollection, nameof(keyElementCollection));

                GenerateKeyHasBeenCalled = true;
                GenerateKeyHasBeenCalledKeyGenerator = keyGenerator;
                GenerateKeyHasBeenCalledKeyElementCollection = keyElementCollection;

                return Task.FromResult(_key);
            }

            #endregion
        }
    }
}