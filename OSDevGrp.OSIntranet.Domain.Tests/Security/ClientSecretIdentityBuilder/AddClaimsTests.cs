using System;
using System.Linq;
using System.Security.Claims;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class AddClaimsTests : ClientSecretIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            Fixture.Customize<Claim>(builder => builder.FromFactory(() => new Claim(Fixture.Create<string>(), Fixture.Create<string>())));
        }

        [Test]
        [Category("UnitTest")]
        public void AddClaims_WhenClaimsIsNull_ThrowsArgumentNullException()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddClaims(null));

            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithIdentifier_WhenCalled_ReturnsClientSecretIdentityBuilder()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentityBuilder result = sut.AddClaims(Fixture.CreateMany<Claim>().ToList());

            Assert.That(result, Is.EqualTo(sut));
        }
    }
}
