using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class UpdateContactGroupAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateContactGroupAsync_WhenContactGroupIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactGroupAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("contactGroup"));
        }
    }
}
