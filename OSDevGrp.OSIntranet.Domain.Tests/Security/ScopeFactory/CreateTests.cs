using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ScopeFactory
{
    [TestFixture]
    public class CreateTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(null, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(string.Empty, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenNameIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(" ", _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenDescriptionIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(_fixture.Create<string>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("description"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenDescriptionIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(_fixture.Create<string>(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("description"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenDescriptionIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ScopeFactory.Create(_fixture.Create<string>(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("description"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IScopeBuilder result = Domain.Security.ScopeFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsScopeBuilder()
        {
            IScopeBuilder result = Domain.Security.ScopeFactory.Create(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<Domain.Security.ScopeBuilder>());
        }
    }
}