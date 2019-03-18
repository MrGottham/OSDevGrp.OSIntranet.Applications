using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetBudgetAccountGroupsAsyncTests
    {
        #region Private variables

        private IConfiguration _configuration;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<GetBudgetAccountGroupsAsyncTests>()
                .Build();
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountGroupsAsync_WhenCalled_ReturnsBudgetAccountGroups()
        {
            IAccountingRepository sut = CreateSut();

            IList<IBudgetAccountGroup> result = (await sut.GetBudgetAccountGroupsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        private IAccountingRepository CreateSut()
        {
            return new Repositories.AccountingRepository(_configuration);
        }
    }
}