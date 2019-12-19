using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.PaymentTermCommandBase
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
        public void ToDomain_WhenCalled_ReturnsPaymentTerm()
        {
            IPaymentTermCommand sut = CreateSut();

            IPaymentTerm result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Domain.Accounting.PaymentTerm>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPaymentTermWithNumberFromCommand()
        {
            int number = _fixture.Create<int>();
            IPaymentTermCommand sut = CreateSut(number);

            int result = sut.ToDomain().Number;

            Assert.That(result, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPaymentTermWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            IPaymentTermCommand sut = CreateSut(name: name);

            string result = sut.ToDomain().Name;

            Assert.That(result, Is.EqualTo(name));
        }

        private IPaymentTermCommand CreateSut(int? number = null, string name = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.PaymentTermCommandBase
        {
        }
    }
}
