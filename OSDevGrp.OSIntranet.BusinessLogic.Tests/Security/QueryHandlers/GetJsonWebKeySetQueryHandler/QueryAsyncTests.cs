using AutoFixture;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetJsonWebKeySetQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IOptions<TokenGeneratorOptions>> _tokenGeneratorOptionsMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenGeneratorOptionsMock = new Mock<IOptions<TokenGeneratorOptions>>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValueWasCalledOnTokenGeneratorOptions()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            _tokenGeneratorOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertSigningAlgorithmWasCalledOnTokenGenerator()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            _tokenGeneratorMock.Verify(m => m.SigningAlgorithm, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result.Keys, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysIsNotEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result.Keys, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKey()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result.Keys.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigning()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result.Keys.Count(jsonWebKey => jsonWebKey.HasPrivateKey == false && string.CompareOrdinal(jsonWebKey.Use, JsonWebKeyUseNames.Sig) == 0), Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningBasedOnJsonWebKeyFromTokenGeneratorOptions()
        {
            JsonWebKey jsonWebKey = CreateJsonWebKey();
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions(jsonWebKey);
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut(tokenGeneratorOptions);

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kty, Is.EqualTo(jsonWebKey.Kty));
            Assert.That(result.N, Is.EqualTo(jsonWebKey.N));
            Assert.That(result.E, Is.EqualTo(jsonWebKey.E));
            Assert.That(result.D, Is.Null);
            Assert.That(result.DP, Is.Null);
            Assert.That(result.DQ, Is.Null);
            Assert.That(result.P, Is.Null);
            Assert.That(result.Q, Is.Null);
            Assert.That(result.QI, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyTypeIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kty, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyTypeIsNotEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kty, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyTypeIsEqualToRSA()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kty, Is.EqualTo(JsonWebAlgorithmsKeyTypes.RSA));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWherePublicKeyUseIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Use, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWherePublicKeyUseIsNotEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Use, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWherePublicKeyUseIsEqualToSig()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Use, Is.EqualTo(JsonWebKeyUseNames.Sig));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyOperationsIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.KeyOps, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyOperationsIsEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.KeyOps, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereAlgorithmIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Alg, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereAlgorithmIsNotEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Alg, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereAlgorithmIsEqualToSigningAlgorithmFromTokenGenerator()
        {
            string signingAlgorithm = _fixture.Create<string>();
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut(signingAlgorithm: signingAlgorithm);

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Alg, Is.EqualTo(signingAlgorithm));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyIdIsNotNull()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kid, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyIdIsNotEmpty()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kid, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasOneJsonWebKeyWithoutPrivateKeyUsedForSigningWhereKeyIdIsEqualToKeyIdForTokenSecurityKey()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKey result = (await sut.QueryAsync(CreateGetJsonWebKeySetQuery())).Keys.Single(jwt => jwt.HasPrivateKey == false && string.CompareOrdinal(jwt.Use, JsonWebKeyUseNames.Sig) == 0);

            Assert.That(result.Kid, Is.EqualTo("f6498959-0000-41a3-bc87-b82ef23d69a1"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnWhereKeysHasNoneJsonWebKeysWithPrivateKey()
        {
            IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> sut = CreateSut();

            JsonWebKeySet result = await sut.QueryAsync(CreateGetJsonWebKeySetQuery());

            Assert.That(result.Keys.Count(jsonWebKey => jsonWebKey.HasPrivateKey), Is.EqualTo(0));
        }

        private IQueryHandler<IGetJsonWebKeySetQuery, JsonWebKeySet> CreateSut(TokenGeneratorOptions tokenGeneratorOptions = null, string signingAlgorithm = null)
        {
            _tokenGeneratorOptionsMock.Setup(m => m.Value)
                .Returns(tokenGeneratorOptions ?? CreateTokenGeneratorOptions());

            _tokenGeneratorMock.Setup(m => m.SigningAlgorithm)
                .Returns(signingAlgorithm ?? _fixture.Create<string>());

            return new BusinessLogic.Security.QueryHandlers.GetJsonWebKeySetQueryHandler(_tokenGeneratorOptionsMock.Object, _tokenGeneratorMock.Object);
        }

        private TokenGeneratorOptions CreateTokenGeneratorOptions(JsonWebKey jsonWebKey = null)
        {
            return new TokenGeneratorOptions
            {
                Key = jsonWebKey ?? CreateJsonWebKey()
            };
        }

        private static JsonWebKey CreateJsonWebKey()
        {
            using RSA rsa = RSA.Create(4096);

            RSAParameters rsaParameters = rsa.ExportParameters(true);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaParameters);

            return JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
        }

        private IGetJsonWebKeySetQuery CreateGetJsonWebKeySetQuery()
        {
            return CreateGetJsonWebKeySetQueryMock().Object;
        }

        private Mock<IGetJsonWebKeySetQuery> CreateGetJsonWebKeySetQueryMock()
        {
            return new Mock<IGetJsonWebKeySetQuery>();
        }
    }
}