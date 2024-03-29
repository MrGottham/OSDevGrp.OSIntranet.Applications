﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.ContactGroup
{
    [TestFixture]
    public class AllowDeletionTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AllowDeletion_WhenCalled_AssertDeletableIsTrue()
        {
            IDeletable sut = CreateSut();

            sut.AllowDeletion();

            Assert.That(sut.Deletable, Is.True);
        }

        private IDeletable CreateSut()
        {
            return new Domain.Contacts.ContactGroup(_fixture.Create<int>(), _fixture.Create<string>());
        }
    }
}
