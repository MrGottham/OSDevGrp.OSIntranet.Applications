using System.Transactions;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Tests.CommandHandlers.CommandHandlerTransactionalBase
{
    [TestFixture]
    public class TransactionOptionsTests : CommandHandlerTransactionalTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void TransactionOptions_WhenCalled_AssertTypeOfTransactionOptions()
        {
            ICommandHandler sut = CreateSut();

            Assert.That(sut.TransactionOptions, Is.TypeOf<TransactionOptions>());
        }

        [Test]
        [Category("UnitTest")]
        public void TransactionOptions_WhenCalled_AssertIsolationLevelIsEqualToReadCommitted()
        {
            ICommandHandler sut = CreateSut();

            TransactionOptions transactionOptions = sut.TransactionOptions;

            Assert.That(transactionOptions.IsolationLevel, Is.EqualTo(IsolationLevel.ReadCommitted));
        }
    }
}