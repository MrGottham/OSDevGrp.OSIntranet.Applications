using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.CreateNationalityCommand
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
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationality()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Nationality>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereNumberIsEqualToNumberFromCreateNationalityCommand()
        {
            int number = _fixture.Create<int>();
            ICreateNationalityCommand sut = CreateSut(number);

            INationality result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereNameIsNotNull()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereNameIsNotEmpty()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.Name, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereNameIsEqualToNameFromCreateNationalityCommand()
        {
            string name = _fixture.Create<string>();
            ICreateNationalityCommand sut = CreateSut(name: name);

            INationality result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereDeletableIsFalse()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.Deletable, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereCreatedByIdentifierIsNull()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.CreatedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereCreatedDateTimeIsEqualToDefaultDateTime()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.CreatedDateTime, Is.EqualTo(default(DateTime)));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereModifiedByIdentifierIsNull()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.ModifiedByIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNationalityWhereModifiedDateTimeIsEqualToDefaultDateTime()
        {
            ICreateNationalityCommand sut = CreateSut();

            INationality result = sut.ToDomain();

            Assert.That(result.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private ICreateNationalityCommand CreateSut(int? number = null, string name = null)
        {
            return CommonCommandFactory.BuildCreateNationalityCommand(number ?? _fixture.Create<int>(), name ?? _fixture.Create<string>());
        }
    }
}