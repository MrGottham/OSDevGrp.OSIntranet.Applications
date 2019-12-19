using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.ContactGroupCommandBase
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
        public void ToDomain_WhenCalled_ReturnsContactGroup()
        {
            IContactGroupCommand sut = CreateSut();

            IContactGroup result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Domain.Contacts.ContactGroup>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactGroupWithNumberFromCommand()
        {
            int number = _fixture.Create<int>();
            IContactGroupCommand sut = CreateSut(number);

            int result = sut.ToDomain().Number;

            Assert.That(result, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactGroupWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            IContactGroupCommand sut = CreateSut(name: name);

            string result = sut.ToDomain().Name;

            Assert.That(result, Is.EqualTo(name));
        }

        private IContactGroupCommand CreateSut(int? number = null, string name = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Commands.ContactGroupCommandBase
        {
        }
    }
}
