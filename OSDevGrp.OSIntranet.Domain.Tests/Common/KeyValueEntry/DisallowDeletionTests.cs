using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Common.KeyValueEntry
{
    [TestFixture]
    public class DisallowDeletionTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void DisallowDeletion_WhenCalled_AssertDeletableIsFalse()
        {
            IDeletable sut = CreateSut();

            sut.DisallowDeletion();

            Assert.That(sut.Deletable, Is.False);
        }

        private IDeletable CreateSut()
        {
            return new Domain.Common.KeyValueEntry(_fixture.Create<string>(), _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray());
        }
    }
}