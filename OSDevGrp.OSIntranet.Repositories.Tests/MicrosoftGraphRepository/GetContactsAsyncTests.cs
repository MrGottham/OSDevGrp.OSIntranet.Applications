﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MicrosoftGraphRepository
{
    [TestFixture]
    public class GetContactsAsyncTests : MicrosoftGraphRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void GetContactsAsync_WhenRefreshableTokenIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContactsAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which communicate with Microsoft Graph should only be executed when we have an access token")]
        public async Task GetContactsAsync_WhenCalled_ReturnsContacts()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            IEnumerable<IContact> result = await sut.GetContactsAsync(CreateToken());

            Assert.That(result.Count(), Is.GreaterThan(0));
        }
    }
}
