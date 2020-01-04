using System;
using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class ApplyContactSupplementAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void ApplyContactSupplementAsync_WhenContactIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyContactSupplementAsync((IContact) null));

            Assert.That(result.ParamName, Is.EqualTo("contact"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyContactSupplementAsync_WhenContactCollectionIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyContactSupplementAsync((IEnumerable<IContact>) null));

            Assert.That(result.ParamName, Is.EqualTo("contacts"));
        }
    }
}
