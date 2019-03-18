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
    public class GetAccountGroupsAsyncTests
    {
        #region Private variables

        private IConfiguration _configuration;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<GetAccountGroupsAsyncTests>()
                .Build();
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountGroupsAsync_WhenCalled_ReturnsAccountGroups()
        {
            IAccountingRepository sut = CreateSut();

            IList<IAccountGroup> result = (await sut.GetAccountGroupsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        private IAccountingRepository CreateSut()
        {
            return new Repositories.AccountingRepository(_configuration);
        }
    }
}