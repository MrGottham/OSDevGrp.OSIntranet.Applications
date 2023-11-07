using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class GetLanguageAsyncTests : CommonRepositoryTestBase
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
        [Category("IntegrationTest")]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetLanguageAsync_WhenLanguageExists_ReturnsNotNull(int number)
        {
            ICommonRepository sut = CreateSut();

            ILanguage result = await sut.GetLanguageAsync(number);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetLanguageAsync_WhenLanguageDoesNotExists_ReturnsNull()
        {
            ICommonRepository sut = CreateSut();

            ILanguage result = await sut.GetLanguageAsync(_random.Next(100, 256));

            Assert.That(result, Is.Null);
        }
    }
}