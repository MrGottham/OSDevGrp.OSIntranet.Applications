using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class GetCountriesAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetCountriesAsync_WhenCalled_ReturnsCountries()
        {
            IContactRepository sut = CreateSut();

            IList<ICountry> result = (await sut.GetCountriesAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
