using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CompanyNameCommand
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
        public void ToDomain_WhenCalled_ReturnsCompanyName()
        {
            ICompanyNameCommand sut = CreateSut();

            IName result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<CompanyName>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCompanyNameWithFullNameFromCommand()
        {
            string fullName = _fixture.Create<string>();
            ICompanyNameCommand sut = CreateSut(fullName);

            string result = ((ICompanyName) sut.ToDomain()).FullName;

            Assert.That(result, Is.EqualTo(fullName));
        }

        private ICompanyNameCommand CreateSut(string fullName = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.CompanyNameCommand>()
                .With(m => m.FullName, fullName ?? _fixture.Create<string>())
                .Create();
        }
    }
}