using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateAccountAsyncTests : AccountingRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateAccountAsync_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("account"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will create an account (should only be used for debugging)")]
        public async Task CreateAccountAsync_WhenCalled_CreatesAccount()
        {
            IAccountingRepository sut = CreateSut();

            IAccounting accounting = await sut.GetAccountingAsync(WithExistingAccountingNumber(), DateTime.Today);
            IAccountGroup[] accountGroupCollection = (await sut.GetAccountGroupsAsync()).ToArray();

            IAccount account = new Account(accounting, WithExistingAccountNumberForAccount(), _fixture.Create<string>(), accountGroupCollection[_random.Next(0, accountGroupCollection.Length - 1)])
            {
                Description = _fixture.Create<string>()
            };
            IAccount result = await sut.CreateAccountAsync(account);

            Assert.That(result, Is.Not.Null);
        }
    }
}