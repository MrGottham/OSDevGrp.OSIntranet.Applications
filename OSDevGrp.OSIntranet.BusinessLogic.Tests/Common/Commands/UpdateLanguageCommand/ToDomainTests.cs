using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.UpdateLanguageCommand
{
    [TestFixture]
    public class ToDomainTests
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
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguage()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Language>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereNumberIsEqualToNumberFromUpdateLanguageCommand()
        {
            int number = _fixture.Create<int>();
            IUpdateLanguageCommand sut = CreateSut(number);

            ILanguage result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereNameIsNotNull()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereNameIsNotEmpty()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereNameIsEqualToNameFromUpdateLanguageCommand()
        {
            string name = _fixture.Create<string>();
            IUpdateLanguageCommand sut = CreateSut(name: name);

            ILanguage result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereDeletableIsFalse()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereCreatedByIdentifierIsNull()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereModifiedByIdentifierIsNull()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLanguageWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
            IUpdateLanguageCommand sut = CreateSut();

            ILanguage result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private IUpdateLanguageCommand CreateSut(int? number = null, string name = null)
        {
            return CommonCommandFactory.BuildUpdateLanguageCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}