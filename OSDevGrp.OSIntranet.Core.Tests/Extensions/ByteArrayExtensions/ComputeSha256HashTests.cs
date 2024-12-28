﻿using AutoFixture;
using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.Core.Tests.Extensions.ByteArrayExtensions
{
    [TestFixture]
    public class ComputeSha256HashTests
    {
        #region Private constants

        private const int ExpectedLength = 32;

        #endregion

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
        public void ComputeSha256Hash_WhenValueIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Core.Extensions.ByteArrayExtensions.ComputeSha256Hash(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsEmpty_ReturnsNotNull()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash([]);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsEmptyByteArray_ReturnsNonEmptyByteArray()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash([]);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsEmptyByteArray_ReturnsNonEmptyByteArrayWithExpectedNumberOfBytes()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash([]);

            Assert.That(result.Length, Is.EqualTo(ExpectedLength));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsEmptyByteArray_ReturnsNonEmptyByteArrayMatchingComputedHashOfValue()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash([]);

            Assert.That(result, Is.EqualTo(ExpectedValue([])));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsNonEmptyByteArray_ReturnsNonEmptyByteArray()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsNonEmptyByteArray_ReturnsNonEmptyByteArrayWithExpectedNumberOfBytes()
        {
            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray());

            Assert.That(result.Length, Is.EqualTo(ExpectedLength));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha256Hash_WhenValueIsNonEmptyByteArray_ReturnsNonEmptyByteArrayMatchingComputedHashOfValue()
        {
            byte[] value = _fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray();

            byte[] result = Core.Extensions.ByteArrayExtensions.ComputeSha256Hash(value);

            Assert.That(result, Is.EqualTo(ExpectedValue(value)));
        }

        private static byte[] ExpectedValue(byte[] value)
        {
            Core.NullGuard.NotNull(value, nameof(value));

            using HashAlgorithm hashAlgorithm = SHA256.Create();

            return hashAlgorithm.ComputeHash(value);
        }
    }
}