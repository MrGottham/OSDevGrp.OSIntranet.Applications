using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class DeleteCountryAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task DeleteCountryAsync_WhenCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task DeleteCountryAsync_WhenCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(string.Empty)))!;

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task DeleteCountryAsync_WhenCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(" ")))!;

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }
    }
}
