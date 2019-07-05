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
        public void DeleteCountryAsync_WhenCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteCountryAsync_WhenCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteCountryAsync_WhenCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteCountryAsync(" "));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }
    }
}
