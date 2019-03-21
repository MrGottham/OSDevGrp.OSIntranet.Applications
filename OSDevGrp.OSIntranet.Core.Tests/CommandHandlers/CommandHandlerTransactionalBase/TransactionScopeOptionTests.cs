using System.Transactions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerTransactionalBase
{
    [TestFixture]
    public class TransactionScopeOptionTests : CommandHandlerTransactionalTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void TransactionScopeOption_WhenCalled_EqualToRequired()
        {
            ICommandHandler sut = CreateSut();

            Assert.That(sut.TransactionScopeOption, Is.EqualTo(TransactionScopeOption.Required));
        }
    }
}