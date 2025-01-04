using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentFactory
{
    [TestFixture]
    public class CreateTest
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenSubjectIdentifierIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("subjectIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenSubjectIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(string.Empty, _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("subjectIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenSubjectIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(" ", _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("subjectIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(_fixture.Create<string>(), null, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("userInfo"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IIdTokenContentFactory sut = CreateSut();

            IIdTokenContentBuilder result = sut.Create(_fixture.Create<string>(), _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsIdTokenContentBuilder()
        {
            IIdTokenContentFactory sut = CreateSut();

            IIdTokenContentBuilder result = sut.Create(_fixture.Create<string>(), _fixture.BuildUserInfoMock().Object, DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1));

            Assert.That(result, Is.TypeOf<BusinessLogic.Security.Logic.IdTokenContentBuilder>());
        }

        private IIdTokenContentFactory CreateSut()
        {
            return new BusinessLogic.Security.Logic.IdTokenContentFactory();
        }
    }
}