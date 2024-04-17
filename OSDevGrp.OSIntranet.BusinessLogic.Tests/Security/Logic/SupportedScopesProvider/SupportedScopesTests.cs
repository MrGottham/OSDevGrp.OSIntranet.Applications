using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SupportedScopesProvider
{
    [TestFixture]
    public class SupportedScopesTests
    {
        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsNotNull()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsNoneEmptyDictionary()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryWithFourScopes()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result.Count, Is.EqualTo(4));
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingKeyForOpenIdScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result.ContainsKey(ScopeHelper.OpenIdScope), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingOpenIdScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IScope result = sut.SupportedScopes[ScopeHelper.OpenIdScope];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(ScopeHelper.OpenIdScope));
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingKeyForProfileScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result.ContainsKey(ScopeHelper.ProfileScope), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingProfileScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IScope result = sut.SupportedScopes[ScopeHelper.ProfileScope];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(ScopeHelper.ProfileScope));
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingKeyForEmailScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result.ContainsKey(ScopeHelper.EmailScope), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingEmailScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IScope result = sut.SupportedScopes[ScopeHelper.EmailScope];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(ScopeHelper.EmailScope));
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingKeyForWebApiScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IReadOnlyDictionary<string, IScope> result = sut.SupportedScopes;

            Assert.That(result.ContainsKey(ScopeHelper.WebApiScope), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SupportedScopes_WhenCalled_ReturnsDictionaryContainingWebApiScope()
        {
            ISupportedScopesProvider sut = CreateSut();

            IScope result = sut.SupportedScopes[ScopeHelper.WebApiScope];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(ScopeHelper.WebApiScope));
        }

        private ISupportedScopesProvider CreateSut()
        {
            return new BusinessLogic.Security.Logic.SupportedScopesProvider();
        }
    }
}