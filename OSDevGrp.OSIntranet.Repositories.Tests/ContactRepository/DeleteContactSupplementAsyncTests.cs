using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    public class DeleteContactSupplementAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void DeleteContactSupplementAsync_WhenExternalIdentifierIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactSupplementAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactSupplementAsync_WhenExternalIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactSupplementAsync(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactSupplementAsync_WhenExternalIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactSupplementAsync(" "));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }
    }
}
