using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SecurityKeyBuilder
{
    [TestFixture]
    public class BuildTests : SecurityKeyBuilderTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsNotNull()
        {
            using ISecurityKeyBuilder sut = CreateSut();

            SecurityKey result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsRsaSecurityKey()
        {
            using ISecurityKeyBuilder sut = CreateSut();

            SecurityKey result = sut.Build();

            Assert.That(result, Is.TypeOf<RsaSecurityKey>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsRsaSecurityKeyWhereParametersIsNotNull()
        {
            using ISecurityKeyBuilder sut = CreateSut();

            RsaSecurityKey result = (RsaSecurityKey) sut.Build();

            Assert.That(result.Rsa.ExportParameters(true), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsRsaSecurityKeyWhereParametersBasedOnJsonWebKey()
        {
            JsonWebKey jsonWebKey = CreateJsonWebKey();
            using ISecurityKeyBuilder sut = CreateSut(jsonWebKey);

            RSAParameters result = ((RsaSecurityKey) sut.Build()).Rsa.ExportParameters(true);

            Assert.That(Base64UrlEncoder.Encode(result.Modulus), Is.EqualTo(jsonWebKey.N));
            Assert.That(Base64UrlEncoder.Encode(result.Exponent), Is.EqualTo(jsonWebKey.E));
            Assert.That(Base64UrlEncoder.Encode(result.D), Is.EqualTo(jsonWebKey.D));
            Assert.That(Base64UrlEncoder.Encode(result.DP), Is.EqualTo(jsonWebKey.DP));
            Assert.That(Base64UrlEncoder.Encode(result.DQ), Is.EqualTo(jsonWebKey.DQ));
            Assert.That(Base64UrlEncoder.Encode(result.P), Is.EqualTo(jsonWebKey.P));
            Assert.That(Base64UrlEncoder.Encode(result.Q), Is.EqualTo(jsonWebKey.Q));
            Assert.That(Base64UrlEncoder.Encode(result.InverseQ), Is.EqualTo(jsonWebKey.QI));
        }
    }
}