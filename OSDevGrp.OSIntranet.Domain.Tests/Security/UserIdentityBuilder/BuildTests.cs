using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentityBuilder
{
    [TestFixture]
    public class BuildTests : UserIdentityBuilderTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_AssertCorrectExternalUserIdentifier()
        {
            string externalUserIdentifier = Fixture.Create<string>();
            IUserIdentityBuilder sut = CreateSut(externalUserIdentifier);

            IUserIdentity result = sut.Build();

            Assert.That(result.ExternalUserIdentifier, Is.EqualTo(externalUserIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasNotBeenCalled_AssertDefaultIdentifier()
        {
            IUserIdentityBuilder sut = CreateSut();

            IUserIdentity result = sut.Build();

            Assert.That(result.Identifier, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhereWithIdentifierHasBeenCalled_AssertCorrectIdentifier()
        {
            IUserIdentityBuilder sut = CreateSut();

            int identifier = Fixture.Create<int>();
            IUserIdentity result = sut.WithIdentifier(identifier).Build();

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }
    }
}
