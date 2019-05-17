using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountGroupCommandBase
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
        public void ToDomain_WhenCalled_ReturnsAccountGroup()
        {
            IAccountGroupCommand sut = CreateSut();

            IAccountGroup result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<AccountGroup>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountGroupWithNumberFromCommand()
        {
            int number = _fixture.Create<int>();
            IAccountGroupCommand sut = CreateSut(number);

            int result = sut.ToDomain().Number;

            Assert.That(result, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountGroupWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            IAccountGroupCommand sut = CreateSut(name: name);

            string result = sut.ToDomain().Name;

            Assert.That(result, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountGroupWithAccountGroupTypeFromCommand()
        {
            AccountGroupType accountGroupType = _fixture.Create<AccountGroupType>();
            IAccountGroupCommand sut = CreateSut(accountGroupType: accountGroupType);

            AccountGroupType result = sut.ToDomain().AccountGroupType;

            Assert.That(result, Is.EqualTo(accountGroupType));
        }

        private IAccountGroupCommand CreateSut(int? number = null, string name = null, AccountGroupType? accountGroupType = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.AccountGroupType, accountGroupType ?? _fixture.Create<AccountGroupType>())
                .Create();
        }
 
        private class Sut : BusinessLogic.Accounting.Commands.AccountGroupCommandBase
        {
        }
   }
}