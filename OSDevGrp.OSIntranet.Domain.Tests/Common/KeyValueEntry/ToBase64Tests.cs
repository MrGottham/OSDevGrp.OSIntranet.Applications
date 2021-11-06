using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Common.KeyValueEntry
{
    [TestFixture]
    public class ToBase64Tests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random();
        }

        [Test]
        [Category("UnitTest")]
        public void ToBase64_WhenCalled_ReturnsBase64String()
        {
            IKeyValueEntry sut = CreateSut();

            string result = sut.ToBase64();

            Assert.That(result.IsBase64String(), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToBase64_WhenCalled_ReturnsBase64StringMatchingValue()
        {
            byte[] value = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            IKeyValueEntry sut = CreateSut(value);

            string base64String = sut.ToBase64();

            Assert.That(base64String, Is.EqualTo(Convert.ToBase64String(value)));
        }

        private IKeyValueEntry CreateSut(byte[] value = null)
        {
            return new Domain.Common.KeyValueEntry(_fixture.Create<string>(), value ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray());
        }
    }
}