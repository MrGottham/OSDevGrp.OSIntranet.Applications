using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    [TestFixture]
    public class BuildTests : ClientSecretIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_AssertCorrectFriendlyName()
        {
            string friendlyName = Fixture.Create<string>();
            IClientSecretIdentityBuilder sut = CreateSut(friendlyName);

            IClientSecretIdentity result = sut.Build();

            Assert.That(result.FriendlyName, Is.EqualTo(friendlyName));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasNotBeenCalled_AssertDefaultIdentifier()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentity result = sut.Build();

            Assert.That(result.Identifier, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasBeenCalled_AssertCorrectIdentifier()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            int identifier = Fixture.Create<int>();
            IClientSecretIdentity result = sut.WithIdentifier(identifier).Build();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithClientIdHasNotBeenCalled_AssertDefaultClientId()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentity result = sut.Build();

            Assert.That(result.ClientId.Length, Is.EqualTo(32));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithClientIdHasBeenCalled_AssertCorrectClientId()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            string clientId = Fixture.Create<string>();
            IClientSecretIdentity result = sut.WithClientId(clientId).Build();

            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithClientSecretHasNotBeenCalled_AssertDefaultClientSecret()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            IClientSecretIdentity result = sut.Build();

            Assert.That(result.ClientSecret.Length, Is.EqualTo(32));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithClientSecretHasBeenCalled_AssertCorrectClientSecret()
        {
            IClientSecretIdentityBuilder sut = CreateSut();

            string clientSecret = Fixture.Create<string>();
            IClientSecretIdentity result = sut.WithClientSecret(clientSecret).Build();

            Assert.That(result.ClientSecret, Is.EqualTo(clientSecret));
        }
    }
}
