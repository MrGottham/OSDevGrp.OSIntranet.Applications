using System.Transactions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerNonTransactionalBase
{
    [TestFixture]
    public class TransactionScopeOptionTests : CommandHandlerNonTransactionalTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void TransactionScopeOption_WhenCalled_EqualToSuppress()
        {
            ICommandHandler sut = CreateSut();

            Assert.That(sut.TransactionScopeOption, Is.EqualTo(TransactionScopeOption.Suppress));
        }
    }
}