using System.Transactions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerNonTransactionalBase
{
    [TestFixture]
    public class TransactionOptionsTests : CommandHandlerNonTransactionalTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void TransactionOptions_WhenCalled_AssertTypeOfTransactionOptions()
        {
            ICommandHandler sut = CreateSut();

            Assert.That(sut.TransactionOptions, Is.TypeOf<TransactionOptions>());
        }
    }
}