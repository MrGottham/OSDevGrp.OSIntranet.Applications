using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class CreateContactGroupAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateContactGroupAsync_WhenContactGroupIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContactGroupAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("contactGroup"));
        }
    }
}
