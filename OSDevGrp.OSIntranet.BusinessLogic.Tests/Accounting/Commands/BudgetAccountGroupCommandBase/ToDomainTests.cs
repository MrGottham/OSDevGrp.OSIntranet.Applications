using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.BudgetAccountGroupCommandBase
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
        public void ToDomain_WhenCalled_ReturnsBudgetAccountGroup()
        {
            IBudgetAccountGroupCommand sut = CreateSut();

            IBudgetAccountGroup result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<BudgetAccountGroup>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountGroupWithNumberFromCommand()
        {
            int number = _fixture.Create<int>();
            IBudgetAccountGroupCommand sut = CreateSut(number);

            int result = sut.ToDomain().Number;

            Assert.That(result, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountGroupWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            IBudgetAccountGroupCommand sut = CreateSut(name: name);

            string result = sut.ToDomain().Name;

            Assert.That(result, Is.EqualTo(name));
        }

        private IBudgetAccountGroupCommand CreateSut(int? number = null, string name = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.BudgetAccountGroupCommandBase
        {
        }
    }
}