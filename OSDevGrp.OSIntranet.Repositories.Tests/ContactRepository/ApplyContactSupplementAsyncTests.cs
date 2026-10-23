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
        public async System.Threading.Tasks.Task ApplyContactSupplementAsync_WhenContactIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyContactSupplementAsync((IContact) null)))!;

            Assert.That(result.ParamName, Is.EqualTo("contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task ApplyContactSupplementAsync_WhenContactCollectionIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyContactSupplementAsync((IEnumerable<IContact>) null)))!;

            Assert.That(result.ParamName, Is.EqualTo("contacts"));
        }
    }
}
