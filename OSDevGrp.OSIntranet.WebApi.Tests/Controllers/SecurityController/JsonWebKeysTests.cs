using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class JsonWebKeysTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<TimeProvider> _timeProviderMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _timeProviderMock = new Mock<TimeProvider>();
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetJsonWebKeySetQuery()
        {
            Controller sut = CreateSut();

            await sut.JsonWebKeys();

            _queryBusMock.Verify(m => m.QueryAsync<IGetJsonWebKeySetQuery, JsonWebKeySet>(It.IsNotNull<IGetJsonWebKeySetQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<JsonWebKeySetModel> result = await sut.JsonWebKeys();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<JsonWebKeySetModel> result = await sut.JsonWebKeys();

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<JsonWebKeySetModel> result = await sut.JsonWebKeys();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResultWithValueNotEqualToNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.JsonWebKeys()).Result;

            Assert.That(result!.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResultWithValueWhereKeysIsNotNull()
        {
            Controller sut = CreateSut();

            JsonWebKeySetModel result = (JsonWebKeySetModel) ((OkObjectResult) (await sut.JsonWebKeys()).Result)!.Value;

            Assert.That(result!.Keys, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResultWithValueWhereKeysIsNotEmpty()
        {
            Controller sut = CreateSut();

            JsonWebKeySetModel result = (JsonWebKeySetModel) ((OkObjectResult) (await sut.JsonWebKeys()).Result)!.Value;

            Assert.That(result!.Keys, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task JsonWebKeys_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResultWithValueWhereKeysIsContainingSpecificKey()
        {
            Guid keyId = Guid.NewGuid();
            Controller sut = CreateSut(CreateJsonWebKey(keyId));

            JsonWebKeySetModel result = (JsonWebKeySetModel) ((OkObjectResult) (await sut.JsonWebKeys()).Result)!.Value;

            Assert.That(result!.Keys.SingleOrDefault(key => string.CompareOrdinal(key.Kid, keyId.ToString("D").ToLower()) == 0), Is.Not.Null);
        }

        private Controller CreateSut(JsonWebKey jsonWebKey = null)
        {
            JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
            jsonWebKeySet.Keys.Add(jsonWebKey ?? CreateJsonWebKey(Guid.NewGuid()));
            jsonWebKeySet.Keys.Add(CreateJsonWebKey(Guid.NewGuid()));
            jsonWebKeySet.Keys.Add(CreateJsonWebKey(Guid.NewGuid()));

            _queryBusMock.Setup(m => m.QueryAsync<IGetJsonWebKeySetQuery, JsonWebKeySet>(It.IsAny<IGetJsonWebKeySetQuery>()))
                .Returns(Task.FromResult(jsonWebKeySet));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _dataProtectionProviderMock.Object, _timeProviderMock.Object);
        }

        private static JsonWebKey CreateJsonWebKey(Guid keyId)
        {
            using RSA rsa = RSA.Create(4096);

            RSAParameters rsaParameters = rsa.ExportParameters(false);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsaParameters);

            JsonWebKey jsonWebKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
            jsonWebKey.KeyId ??= keyId.ToString("D").ToLower();
            return jsonWebKey;
        }
    }
}