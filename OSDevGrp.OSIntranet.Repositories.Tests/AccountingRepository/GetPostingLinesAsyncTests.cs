using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetPostingLinesAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetPostingLinesAsync_WhenAccountingNumberExists_ReturnsNonEmptyPostingLineCollection()
        {
            IAccountingRepository sut = CreateSut();

            const int numberOfPostingLines = 25;
            IPostingLineCollection result = await sut.GetPostingLinesAsync(WithExistingAccountingNumber(), DateTime.Today, numberOfPostingLines);

            int count = result.Count();
            Assert.That(count, Is.GreaterThan(0));
            Assert.That(count, Is.LessThanOrEqualTo(numberOfPostingLines));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetPostingLinesAsync_WhenAccountingNumberDoesNotExist_ReturnsEmptyPostingLineCollection()
        {
            IAccountingRepository sut = CreateSut();

            IPostingLineCollection result = await sut.GetPostingLinesAsync(WithNonExistingAccountingNumber(), DateTime.Today, 250);

            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}